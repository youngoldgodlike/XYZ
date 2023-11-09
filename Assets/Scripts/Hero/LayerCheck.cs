using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Hero _hero;
    private Collider2D _collider;
    public bool isTouchingLayer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hero != null)
        {
            if (other.gameObject.tag.Equals("Ground") && _hero.rigidbody.velocity.y < -15)
                _hero.SpawnParticles("Fall");
            if (other.gameObject.tag.Equals("Ground") && !_hero.allowDoubleJump)
                _hero.SpawnParticles("Fall");
        }
        else
        {
            isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        isTouchingLayer = _collider.IsTouchingLayers(_layer);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        isTouchingLayer = _collider.IsTouchingLayers(_layer);
    }

}
