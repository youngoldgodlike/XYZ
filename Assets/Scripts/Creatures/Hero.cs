using Assets.Scripts.Hero;
using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using Components;
using Creatures;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : Creature
{
    [Space] [Header("Params")]
    [SerializeField] private GameBehavior _gameBehavior;

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

    public bool allowDoubleJump => _allowDoubleJump;
    
    private static readonly int ThrowKey = Animator.StringToHash("IsThrow");
    private static readonly int IsOnWall = Animator.StringToHash("IsOnWall");
    
    private GameSession _session;
    private bool _isOnWall;
    private float _defaultGravityScale;


    private void Awake()
    {
        _defaultGravityScale = rigidbody.gravityScale;
    }

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        UpdateHeroWeapon();
        var health = GetComponent<HealthComponent>();
        health.SetHealth(_session.Data.Hp);
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
        
        _animator.SetBool(IsOnWall, _isOnWall);
    }

    protected override void FixedUpdate()
    {
        var xVelocity = Direction.x * _speed ;
        var yVelocity = CalculateYVelocity();
        
        rigidbody.velocity = new Vector2(xVelocity, yVelocity);     
        _animator.SetBool(IsGroundKey, base.IsGrounded);
        _animator.SetFloat(VerticalVelocity, rigidbody.velocity.y);
        _animator.SetBool(IsRunningKey, Direction.x != 0);
        UpdateSpriteDirection(Direction);

        if (yVelocity == 0 && !base.IsGrounded)
        {
            _particles.Spawn("Fall");
        }
    }

    public void AddWeapon()
    {
        _session.Data.AmountWeapon++;
        UpdateHeroWeapon();
    }

    public void OnHealthChanged(int currentHealth)
    {
        _session.Data.Hp = currentHealth;
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
            _particles.Spawn("Jump");
            yVelocity += _jumpForce;
        } 
        else if (_allowDoubleJump)
        {
            _particles.Spawn("Jump");
            yVelocity = _jumpForce;
            _allowDoubleJump = false;
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

        if (_gameBehavior.coinsCount > 0 )
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
    
    public void SpawnAttack1Particle() => _particles.Spawn("Attack");
    public void SpawnParticles(string particleName) => _particles.Spawn(particleName);
    
    private void UpdateHeroWeapon()
    {
        _animator.runtimeAnimatorController = _session.Data.AmountWeapon > 0 ? _armed : _unarmed;
    }


    public void OnDoThrow()
    {
        _particles.Spawn("Throw");
        _session.Data.AmountWeapon--;
    }
    
    public void Throw()
    {
        if (_throwCooldown.IsReady && _session.Data.AmountWeapon > 1)
        {
            _animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }
    }
}
