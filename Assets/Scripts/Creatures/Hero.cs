using System;
using Assets.Scripts.Component;
using Assets.Scripts.Extensions;
using Assets.Scripts.Hero;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Data;
using Assets.Scripts.Utils;
using Creatures;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class Hero : Creature, ICanAddInInventory
    {
        
        [Header("HeroMovement")] 
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private bool _allowDoubleJump;
        [SerializeField] private float _slamDownVelocity;

        [Header("Attack")]
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private Cooldown _attackCooldown;
    
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        [Header("Animators")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Header("Checkers")]
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        public static Action OnAddInInventory;
        
        private GameSession _session;
        private bool _isOnWall;
        private float _defaultGravityScale;
        
        private int SwordsCount => _session.Data.Inventory.Count("Sword");
        private int CoinsCount => _session.Data.Inventory.Count("Coins");
        private int HealthPointCount => _session.Data.Inventory.Count("HealthPotion");

        private static readonly int ThrowKey = Animator.StringToHash("IsThrow");
        private static readonly int IsOnWall = Animator.StringToHash("IsOnWall");
        
        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            
            UpdateHeroWeapon();
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Hp.Value);
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void Update()
        {
            base.IsGrounded = IsGrounded();

            var moveToSameDirection = Direction.x * transform.lossyScale.x != 0;
        
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            Animator.SetBool(IsOnWall, _isOnWall);
        }

        protected override void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed ;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);     
            
            Animator.SetBool(IsGroundKey, base.IsGrounded);
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);
            
            UpdateSpriteDirection(Direction);
        }

        public void AddWeapon()
        {
            _session.Data.Inventory.Add("Sword", 1);
            UpdateHeroWeapon();
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
            OnAddInInventory?.Invoke();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        public override void Attack()
        {
            if (SwordsCount <= 0) return;

            if (_attackCooldown.IsReady)
            {
                base.Attack();
                _attackCooldown.Reset();
            }    
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinsCount > 0)
                SpawnCoins();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void SpawnAttack1Particle() => Particles.Spawn("Attack");

        public void SpawnParticles(string particleName) => Particles.Spawn(particleName);

        public void OnDoThrow()
        {
            DoThrowEffects();
            _session.Data.Inventory.Remove("Sword", 1);
        }

        public void Throw()
        {
            if (_throwCooldown.IsReady && SwordsCount > 1)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }
        }

        public void UseHealthPotion()
        {
            if (HealthPointCount > 0)
            {
                var health = GetComponent<HealthComponent>();
                health.ApplyValueHealth(5);
                _session.Data.Inventory.Remove("HealthPotion", 1);
                OnAddInInventory?.Invoke();
                Debug.Log("Хилл прошел");
            }
        }

        protected override float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpingPressing = Direction.y > 0;
        
            if (base.IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpingPressing && _isOnWall)
            {
                return 0f;
            }

            return  base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
             if( !IsGrounded() && _allowDoubleJump) 
             { 
                 DoJumpEffects();
                 _allowDoubleJump = false;
                 return _jumpSpeed;
             }
             
             return base.CalculateJumpVelocity(yVelocity);
        }


        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;   
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        { 
            Handles.color = IsGrounded() ? HandlesUtils.transparentGreen : HandlesUtils.transparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta,Vector3.forward, _groundCheckRadius);
        }
#endif
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity )
                {
                    Particles.Spawn("Fall");
                }
            }
        }

        private void SpawnCoins()
        {
            var numCoinsDispose = Mathf.Min(CoinsCount , 5);
            _session.Data.Inventory.Remove("Coins", numCoinsDispose); 

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
    
        private void UpdateHeroWeapon()
        {
            var numSwords = _session.Data.Inventory.Count("Sword");
        
            Animator.runtimeAnimatorController = SwordsCount > 0 ? _armed : _unarmed;
        }

        private void DoThrowEffects()
        {
            Particles.Spawn("Throw");
            Sounds.Play("Range");
        }
        
    }
    
}
