using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _OnChange;
        
        private int _maxHealth;

        private void Awake()
        {
            _maxHealth = _health;
        }

        public void ApplyValueHealth(int healthValue)
        {
            
            _health += healthValue;
            _OnChange?.Invoke(_health);

            if (healthValue < 0 )
                _onDamage?.Invoke();
            
            if (_health > _maxHealth)
                _health = _maxHealth;
            
            if (_health <= 0)
                _onDie?.Invoke();
        }

        public void SetHealth(int healthValue) => _health = healthValue;

        
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int> 
    {

    }
}