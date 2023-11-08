﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private UnityEvent _action;
    [SerializeField] private OnTriggerAction _onTriggerAction;
    
    private void OnTriggerEnter2D(Collider2D other)
    {       
        if (other.gameObject.tag == _tag)
        {
            _action?.Invoke();
            _onTriggerAction?.Invoke(other.gameObject);
        }      
    }

    [Serializable]
    public class OnTriggerAction : UnityEvent<GameObject>
    { }
}
