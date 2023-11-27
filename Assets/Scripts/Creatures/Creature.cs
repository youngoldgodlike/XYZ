using System.Collections.Generic;
using Assets.Scripts.Component;
using Assets.Scripts.Component.Audio;
using Assets.Scripts.Creatures;
using Assets.Scripts.Hero;
using UnityEngine;

namespace Creatures
{
    [SelectionBase]
    public class Creature : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] protected float _damageVelocity;
        [SerializeField] private bool _invertScale;

        [SerializeField] protected LayerCheck _groundCheck;
        [SerializeField] protected List<CheckCircleOverlap> _attackRanges;

        protected SpawnListComponent Particles;
        protected PlaySoundsComponent Sounds;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected Vector2 Direction;

        protected bool IsGrounded;
        protected bool IsJumping;
        private MobAI _ai;

        protected static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        protected static readonly int IsGroundKey = Animator.StringToHash("IsGround");
        protected static readonly int IsRunningKey = Animator.StringToHash("IsRun");
        protected static readonly int AttackKey = Animator.StringToHash("IsAttack");
        protected static readonly int Hit = Animator.StringToHash("IsHit");

        protected virtual void Awake()
        {
            Particles = GetComponent<SpawnListComponent>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
            _ai = GetComponent<MobAI>();
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        protected virtual void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed ;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);     
            
            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);

            if (yVelocity < 0 && !IsGrounded)
            {
                Particles.Spawn("Fall");
            }
            
            UpdateSpriteDirection(Direction);
        }

        protected  virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpingPressing = Direction.y > 0;
            
            if (IsGrounded)
            {
                IsJumping = false;
            }
            
            if (isJumpingPressing)
            {
                IsJumping = true;
                
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;

            }
            else if (Rigidbody.velocity.y > 0 && IsJumping)
            {
                yVelocity *= 0.5f;           
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity += _jumpSpeed;
                DoJumpEffects();
            }
            return yVelocity;
        }

        protected void DoJumpEffects()
        {
            Particles.Spawn("Jump");
            Sounds.Play("Jump");
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
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);
            _ai?.StartState(_ai.OnHit());
        }

        public virtual void DefaultAttack()
        {
            foreach (var overlap in _attackRanges)
                overlap.Check();

            Sounds?.Play("Melee");
        }

        public void SetDirection(Vector2 direction) => Direction = direction;
        public virtual void Attack() => Animator.SetTrigger(AttackKey);

        public void SpawnFootDust() => Particles.Spawn("FootStep");

        public void SpawnAttackParticle() => Particles.Spawn("Attack");
    }
    
}