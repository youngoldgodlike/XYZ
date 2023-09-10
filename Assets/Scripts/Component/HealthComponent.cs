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
        private int _maxHealth;

        private void Awake()
        {
            _maxHealth = _health;
        }

        public void ApplyHealth(int healthValue)
        {
            _health += healthValue;

            if (_health > _maxHealth)
                _health = _maxHealth;
            
            Debug.Log($"здоровье:{_health}");
        }
        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            _onDamage?.Invoke();

            if (_health <= 0)
                _onDie?.Invoke();
            
            Debug.Log($"здоровье:{_health}");
        }
    }
}