﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

namespace Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _valueChangeHealth;

        public void ChangeHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                if (gameObject.tag == "Health")
                {
                    healthComponent.ApplyHealth(_valueChangeHealth);
                }
                else if (gameObject.tag == "Damage")
                {
                    healthComponent.ApplyDamage(_valueChangeHealth);
                }
            }
        }
    }
}
