﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private UnityEvent _action;       

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_tag))            
                _action?.Invoke();         
        }

    
    }
}
