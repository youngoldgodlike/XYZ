using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Hero _hero;
    private Collider2D _collider;
    public bool isTouchingLayer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Ground") && _hero.rigidBody.velocity.y < -15)
        {
            _hero.SpawnFallParticle();
        }

        if (other.gameObject.tag.Equals("Ground") && !_hero.allowDoubleJump)
        {
            _hero.SpawnFallParticle();
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }

}
