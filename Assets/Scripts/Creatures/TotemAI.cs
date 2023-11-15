using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class TotemAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private List<TotemHeadAI> _heads ;
        [SerializeField] private Cooldown _totemAttackCooldown;
        [SerializeField] private float _headAttackCooldown;
    
        private void Start()
        {
            _canAttack = GetComponent<LayerCheck>();
            CachedHeads();
        }
    
        private void Update()
        {
            if (_canAttack.isTouchingLayer)
            {
                if (_totemAttackCooldown.IsReady)
                {
               
                    StartCoroutine(Attack());
                    _totemAttackCooldown.Reset();
                }
            }
        }

        private void CachedHeads()
        {
            foreach (Transform head in gameObject.transform)
                if (head.gameObject.TryGetComponent<TotemHeadAI>(out TotemHeadAI headAi))
                    _heads.Add(headAi);
        }

        private IEnumerator Attack()
        {
            foreach (var head in _heads)
            {
                if (head != null)
                {
                    head.Attack();
                    yield return new WaitForSeconds(_headAttackCooldown);
                }
            }
        }

    }
}
