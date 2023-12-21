using System;
using UnityEngine;
using UnityEngine.Events;
namespace Assets.Scripts.Component
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private IntHealthChangeEvent _onIntHealthChange;
        [SerializeField] private FloatHealthChangeEvent _onFloatHealthChange;
        
        private int _maxHealth;

        private void Awake()
        {
            _maxHealth = _health;
        }

        public void ApplyValueHealth(int healthValue)
        {
            
            if (_health <= 0) return;
            
            _health += healthValue;
            if (healthValue < 0 )
                _onDamage?.Invoke();
            
            if (_health > _maxHealth)
                _health = _maxHealth;
            
            if (_health <= 0)
                _onDie?.Invoke();
            
            _onIntHealthChange?.Invoke(_health);
            _onFloatHealthChange?.Invoke(_health, _maxHealth);
        }
        
        public void SetHealth(int healthValue) => _maxHealth = _health = healthValue;
    }

    [Serializable]
    public class IntHealthChangeEvent : UnityEvent<int> 
    {}
    [Serializable]
    public class FloatHealthChangeEvent : UnityEvent<float , float> 
    {}
}