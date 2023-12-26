using System;
using System.Collections;
using Assets.Scripts.Component;
using Assets.Scripts.Extensions;
using Assets.Scripts.Hero;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Data;
using Assets.Scripts.Models.Difinitions;
using Assets.Scripts.Utils;
using Creatures;
using UnityEditor;
using UnityEngine;
using AnimatorController = UnityEditor.Animations.AnimatorController;

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
        [SerializeField] private float _superThrowDelay;
        [SerializeField] private Cooldown _superThrowCooldown;
    
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private int _superThrowParticles;

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
        private HealthComponent _healthComponent;
        private bool _isOnWall;
        private bool _superThrow;
        private float _defaultGravityScale;
        private const string SwordId = "Sword";

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
        private int SwordsCount => _session.Data.Inventory.Count("Sword");
        private int CoinsCount => _session.Data.Inventory.Count("Coins");
        private int HealthPointCount => _session.Data.Inventory.Count("HealthPotion");

        private static readonly int ThrowKey = Animator.StringToHash("IsThrow");
        private static readonly int IsOnWall = Animator.StringToHash("IsOnWall");
        
        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordsCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        private bool CanUseHeal
        {
            get
            {
                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Healing);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            _healthComponent = GetComponent<HealthComponent>();
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
        
        public void OnDoThrow()
        {
            if (_superThrow)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
        
                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }
        
            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");

            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();

            _session.Data.Inventory.Remove(throwableId, 1);
        }

        public void UseHeal()
        {

            if (CanUseHeal && !_healthComponent.IsFullHealth())
            {
                Debug.Log("Нажато");
                var healingId = _session.QuickInventory.SelectedItem.Id;
                var healingDef = DefsFacade.I.HealingItems.Get(healingId);

                switch (healingDef.HealingTag)
                {
                    case HealingTag.SmallHeal:
                        _healthComponent.ApplyValueHealth(3);
                        break;
                    case HealingTag.MediumHeal:
                        _healthComponent.ApplyValueHealth(5);
                        break;
                    case HealingTag.HighHeal:
                        _healthComponent.ApplyValueHealth(7);
                        break;
                }
                
                _session.Data.Inventory.Remove(healingId, 1);
            }
        }

        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || !CanThrow) return;

            if (_superThrowCooldown.IsReady) _superThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }
        

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
    }
    
}
