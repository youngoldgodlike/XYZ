using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Hero;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [SelectionBase]
    public class TotemAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private Cooldown _totemAttackCooldown;
        [SerializeField] private float _headAttackCooldown;

        private List<TotemHeadAI> _heads = new List<TotemHeadAI>();

        private void Awake()
        {
            CachedHeads();
            _canAttack = GetComponent<LayerCheck>();
        }
    
        private void Update()
        {
            if (_canAttack.IsTouchingLayer)
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
            foreach (var head in _heads.ToArray())
            {
                if (head == null)
                {
                    _heads.Remove(head);
                    continue;
                }

                head.Attack();
                yield return new WaitForSeconds(_headAttackCooldown);
                
            }
        }

    }
}
