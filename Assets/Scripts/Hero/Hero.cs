using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerCheck _layerCheck;

    private Rigidbody2D _rigidBody;
    private Vector2 _direction;


    [Header("Setting jump detection")]
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPositionDelta;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;     
    }

   

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(_direction.x * _speed, _rigidBody.velocity.y  );

        var isJumping = _direction.y > 0;

        if (isJumping)
        {
            if(IsGrounded())
                _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        else if (_rigidBody.velocity.y > 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y * 0.4f);
        }
        
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
}
