using Assets.Scripts.Hero;
using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using Components;
using Creatures;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class Hero : Creature
    {
        [Header("HeroMovement")]
        [SerializeField] private bool _allowDoubleJump;

        [Header("Attack")]
        [SerializeField] private Cooldown _throwCooldown;
    
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

        private GameSession _session;
        private bool _isOnWall;
        private float _defaultGravityScale;
        public bool AllowDoubleJump => _allowDoubleJump;
        private int SwordsCount => _session.Data.Inventory.Count("Sword");
        private int CoinsCount => _session.Data.Inventory.Count("Coins");
        private int HealthPointCount => _session.Data.Inventory.Count("HealthPotion");

        private static readonly int ThrowKey = Animator.StringToHash("IsThrow");
        private static readonly int IsOnWall = Animator.StringToHash("IsOnWall");
        
        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            
            UpdateHeroWeapon();
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Hp);
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }
        
        private void Update()
        {
            base.IsGrounded = IsGrounded();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
        
            if (_wallCheck.isTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Debug.Log(_isOnWall);
                rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                rigidbody.gravityScale = _defaultGravityScale;
            }
            Animator.SetBool(IsOnWall, _isOnWall);
        }

        protected override void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed ;
            var yVelocity = CalculateYVelocity();
        
            rigidbody.velocity = new Vector2(xVelocity, yVelocity);     
            Animator.SetBool(IsGroundKey, base.IsGrounded);
            Animator.SetFloat(VerticalVelocity, rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);
            UpdateSpriteDirection(Direction);

            if (yVelocity == 0 && !base.IsGrounded)
            {
                _particles.Spawn("Fall");
            }
        }

        public void AddWeapon()
        {
            _session.Data.Inventory.Add("Sword", 1);
            UpdateHeroWeapon();
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        public override void Attack()
        {
            if (SwordsCount <= 0) return;
        
            base.Attack();    
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

        public void SpawnAttack1Particle() => _particles.Spawn("Attack");

        public void SpawnParticles(string particleName) => _particles.Spawn(particleName);
        
        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
            _session.Data.Inventory.Remove("Sword", 1);
        }

        public void Throw()
        {
            if (_throwCooldown.IsReady && SwordsCount > 1)
            {
                Sounds.Play("Range");
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
                Debug.Log("Хилл прошел");
            }
        }

        protected override float CalculateYVelocity()
        {
            var yVelocity = rigidbody.velocity.y;
            var isJumpingPressing = Direction.y > 0;
        
            if (base.IsGrounded)
            {
                _allowDoubleJump = true;
            }
        
            if (isJumpingPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;           
            }
        
            return yVelocity;
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = rigidbody.velocity.y <= 0.001f;
        
            if (!isFalling)
            {
                return yVelocity;
            }
            if (base.IsGrounded && !_isOnWall)
            {
               DoJumpVfx();
                yVelocity += _jumpForce;
            } 
            else if (_allowDoubleJump)
            {
                DoJumpVfx();
                yVelocity = _jumpForce;
                _allowDoubleJump = false;
            }
            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
        }

        private bool IsGrounded()
        {
            return _groundCheck.isTouchingLayer;   
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        { 
            Handles.color = IsGrounded() ? HandlesUtils.transparentGreen : HandlesUtils.transparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta,Vector3.forward, _groundCheckRadius);
        }
#endif
    
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
    }
    
}
