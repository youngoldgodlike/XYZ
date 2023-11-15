 using UnityEngine;

namespace Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _valueChangeHealth;

        public void ChangeHealth(GameObject target)
        {
            if (target != null)
            {
                var healthComponent = target.GetComponent<HealthComponent>();
                if (healthComponent != null)
                    healthComponent.ApplyValueHealth(_valueChangeHealth);
            }
            else
            {
                var healthComponent = GameObject.Find("HERO").GetComponent<HealthComponent>();
                healthComponent.ApplyValueHealth(_valueChangeHealth);
            }          
        }
    }
}
