using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerCheck _layerCheck;
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rigidBody;
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;
    private static readonly int IsGroundKey = Animator.StringToHash("IsGround");
    private static readonly int IsRunningKey = Animator.StringToHash("IsRun");
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");


    [Header("Setting jump detection")]
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPositionDelta;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;     
    }

   

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(_direction.x * _speed, _rigidBody.velocity.y  );

        var isJumping = _direction.y > 0;
        var isGrounded = IsGrounded();

        if (isJumping)
        {
            if(isGrounded)
                _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        else if (_rigidBody.velocity.y > 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y * 0.4f);
        }
        _animator.SetBool(IsGroundKey, isGrounded);
        _animator.SetFloat(VerticalVelocity, _rigidBody.velocity.y);
        _animator.SetBool(IsRunningKey, _direction.x != 0);

        UpdateSpriteDirection();
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
            _spriteRenderer.flipX = false;
        }
        else if (_direction.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
