 using UnityEngine;

namespace Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _valueChangeHealth;

        public void ChangeHealth(GameObject target)
        {
            if (target.TryGetComponent(out HealthComponent health))
                health.ApplyValueHealth(_valueChangeHealth);
        }
    }
}
