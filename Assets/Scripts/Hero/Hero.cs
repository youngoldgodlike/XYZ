using Components;
using UnityEngine;
using UnityEngine.Serialization;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _damageJumpSpeed;
    [SerializeField] private LayerCheck _layerCheck;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _interactionRadius; 
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private SpawnComponent _footStepParticles;
    [SerializeField] private SpawnComponent _fallParticles;
    [SerializeField] private GameBehavior _gameBehavior;
    [SerializeField] private SpawnComponent _jumpParticles;
    [SerializeField] private ParticleSystem _hitParticles;

    
    public Rigidbody2D rigidBody;
    public bool allowDoubleJump;
    
    private Vector2 _direction;
    private bool _isGrounded;
    private Collider2D[] _interactionResult = new Collider2D[1];
    
    private static readonly int IsGroundKey = Animator.StringToHash("IsGround");
    private static readonly int IsRunningKey = Animator.StringToHash("IsRun");
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
    private static readonly int Hit = Animator.StringToHash("IsHit");


    [Header("Setting jump detection")]
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPositionDelta;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;     
    }
   private void Update()
    {
        _isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        var xVelocity = _direction.x * _speed ;
        var yVelocity = CalculateYVelocity();
        
        rigidBody.velocity = new Vector2(xVelocity, yVelocity);     
        _animator.SetBool(IsGroundKey, _isGrounded);
        _animator.SetFloat(VerticalVelocity, rigidBody.velocity.y);
        _animator.SetBool(IsRunningKey, _direction.x != 0);
        UpdateSpriteDirection();

        if (yVelocity == 0 && !_isGrounded)
        {
            SpawnFallParticle();
        }
    }

    private float CalculateYVelocity()
    {
        var yVelocity = rigidBody.velocity.y;
        var isJumpingPressing = _direction.y > 0;
        
        if (_isGrounded)
        {
            allowDoubleJump = true;
        }
        
        if (isJumpingPressing)
        {
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (rigidBody.velocity.y > 0)
        {
            yVelocity *= 0.5f;           
        }
        
        
        return yVelocity;
    }
    private float CalculateJumpVelocity(float yVelocity)
    {
        var isFalling = rigidBody.velocity.y <= 0.001f;
        
        if (!isFalling)
        {
            return yVelocity;
        }
        if (_isGrounded)
        {
            _jumpParticles.Spawn();
            yVelocity += _jumpForce;
        } 
        else if (allowDoubleJump)
        {
            _jumpParticles.Spawn();
            yVelocity = _jumpForce;
            allowDoubleJump = false;
            
        }
        return yVelocity;

    }
    private bool IsGrounded()
    {
        return _layerCheck.isTouchingLayer;   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position , 0.3f);
    }

    public void SaySomething()
    {
        Debug.Log("Something!");
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1 ,1);
        }
    }

    public void TakeDamage()
    {
            _animator.SetTrigger(Hit);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, _damageJumpSpeed);

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
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult,_interactionLayer);

        for (int i = 0; i <  size; i++)
        {
           var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
           if (interactable != null)
           {
               interactable.Interact();
           }
        }
    }

    public void SpawnFootDust()
    {
        _footStepParticles.Spawn();
    }

    public void SpawnFallParticle()
    {
        _fallParticles.Spawn();
    }

    
    
    
}
