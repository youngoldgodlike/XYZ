using Assets.Scripts.Hero;
using Component;
using UnityEngine;

namespace Creatures
{
    public class Creature : MonoBehaviour
    {
        public Rigidbody2D rigidbody;
        public bool _invertScale;
        
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] protected float _damageVelocity;

        [SerializeField] protected LayerCheck _groundCheck;
        
        [SerializeField] protected CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;
        
        [SerializeField] protected Animator _animator;
        protected bool IsGrounded;
        protected Vector2 Direction;

        protected static readonly int IsGroundKey = Animator.StringToHash("IsGround");
        protected static readonly int IsRunningKey = Animator.StringToHash("IsRun");
        protected static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        protected static readonly int Hit = Animator.StringToHash("IsHit");
        protected static readonly int AttackKey = Animator.StringToHash("IsAttack");

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.isTouchingLayer;
        }

        protected  virtual float CalculateYVelocity()
        {
            var yVelocity = rigidbody.velocity.y;
            var isJumpingPressing = Direction.y > 0;

            if (isJumpingPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
                var isFalling = rigidbody.velocity.y <= 0.001f;
        
                if (!isFalling)
                {
                    return yVelocity;
                }
            }
            else if (rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;           
            }

            return yVelocity;
        }

        protected virtual void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed ;
            var yVelocity = CalculateYVelocity();
        
            rigidbody.velocity = new Vector2(xVelocity, yVelocity);     
            _animator.SetBool(IsGroundKey, IsGrounded);
            _animator.SetFloat(VerticalVelocity, rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, Direction.x != 0);

            if (yVelocity == 0 && !IsGrounded)
            {
                _particles.Spawn("Fall");
            }
            
            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity += _jumpForce;
                _particles.Spawn("Jump");
            }
            return yVelocity;
        }

        public virtual void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-multiplier, 1 ,1);
            }
        }

        public virtual void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, _damageVelocity);
        }

        public virtual void Attack()
        {
            _animator.SetTrigger(AttackKey);
        }

        public void DefaultAttack()
        {
            _attackRange.Check();
        }

        public void SpawnFootDust() => _particles.Spawn("FootStep");
        public void SetDirection(Vector2 direction) => Direction = direction;
    }
    
}