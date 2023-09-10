﻿using UnityEngine;

namespace Components
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] string _animationKey;

        public void Switch()
        {
            _state = !_state;
            _animator.SetBool(_animationKey, _state);
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}