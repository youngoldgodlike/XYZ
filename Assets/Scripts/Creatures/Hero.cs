using Assets.Scripts.Hero;
using Assets.Scripts.Models;
using Components;
using Creatures;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : Creature
{
   
    public bool allowDoubleJump;

    [Space]
    [Header("Parametrs")]
    [SerializeField] private CheckCircleOverlap _interactionCheck;
    [SerializeField] private float _interactionRadius; 
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private GameBehavior _gameBehavior;
    
    [Space]
    [Header("Particles")]
    [SerializeField] private ParticleSystem _hitParticles;
    
    [Space]
    [Header("Animators")]
    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _unarmed;

    [Space]
    [Header("Setting jump detection")]
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPositionDelta;

    [Space]
    [Header("Attack")]
    
    protected static readonly int ThrowKey = Animator.StringToHash("IsThrow");
    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        UpdateHeroWeapon();
        var health = GetComponent<HealthComponent>();
        health.SetHealth(_session.Data.Hp);
    }

    public void OnHealthChanged(int currentHealth)
    {
        _session.Data.Hp = currentHealth;
    }
    
   private void Update()
    {
        base.IsGrounded = IsGrounded();
    }
   
   protected override void FixedUpdate()
   {
       var xVelocity = Direction.x * _speed ;
       var yVelocity = CalculateYVelocity();
        
       rigidbody.velocity = new Vector2(xVelocity, yVelocity);     
       _animator.SetBool(IsGroundKey, base.IsGrounded);
       _animator.SetFloat(VerticalVelocity, rigidbody.velocity.y);
       _animator.SetBool(IsRunningKey, Direction.x != 0);
       UpdateSpriteDirection();

       if (yVelocity == 0 && !base.IsGrounded)
       {
           _particles.Spawn("Fall");
       }
       
   }
    protected override float CalculateYVelocity()
    {
        var yVelocity = rigidbody.velocity.y;
        var isJumpingPressing = Direction.y > 0;
        
        if (base.IsGrounded)
        {
            allowDoubleJump = true;
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
        if (base.IsGrounded)
        {
            _particles.Spawn("Jump");
            yVelocity += _jumpForce;
        } 
        else if (allowDoubleJump)
        {
            _particles.Spawn("Jump");
            yVelocity = _jumpForce;
            allowDoubleJump = false;
        }
        return yVelocity;
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

    public void Attack()
    {
        if (!_armed) return;

        _animator.SetTrigger(AttackKey);       
    }

    public override void TakeDamage()
    {
        _animator.SetTrigger(Hit);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, _damageVelocity);

        if (_gameBehavior.coinsCount > 0)
            SpawnCoins();
    }

    public void SpawnCoins()
    {
        var numCoinsDispose = Mathf.Min(_gameBehavior.coinsCount , 5);
        _gameBehavior.coinsCount -= numCoinsDispose;

        var burst = _hitParticles.emission.GetBurst(0);
        burst.count = numCoinsDispose;
        _hitParticles.emission.SetBurst(0, burst);

        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
        Debug.Log(_gameBehavior.coinsCount);
    }
    
   
    public void Interact()
    {
        _interactionCheck.Check();
    }

    public void SpawnFootDust() => _particles.Spawn("FootStep");

    public void SpawnAttack1Particle() => _particles.Spawn("Attack");
    public void SpawnParticles(string particleName) => _particles.Spawn(particleName);
    
    
    public void ArmHero()
    {
        _session.Data.IsArmed = true;
        UpdateHeroWeapon();      
    }

    private void UpdateHeroWeapon()
    {
        _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unarmed;
    }


    public void OnDoThrow()
    {
        _particles.Spawn("Throw");
    }
    
    public void Throw()
    {
        _animator.SetTrigger(ThrowKey);
    }
}
