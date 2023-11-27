using System;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private LayerMask _layer = ~0;

    [SerializeField] private OnTriggerAction _onTriggerAction;
    [SerializeField] private UnityEvent _action;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.IsInLayer(_layer)) return;
        if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;
        _action?.Invoke();
        _onTriggerAction?.Invoke(other.gameObject);
    }

    [Serializable]
    public class OnTriggerAction : UnityEvent<GameObject>
    { }
}
