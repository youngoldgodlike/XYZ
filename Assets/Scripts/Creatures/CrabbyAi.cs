using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class CrabbyAi : MobAI
    {
        [SerializeField] private Cooldown _attackDelay;
        protected override IEnumerator GoToTarget()
        {
            if (IsDead) yield break;

            while (_vision.IsTouchingLayer )
            {
                SetDirectionToTarget();
                AttackAndRun();
                
                yield return default;
            }
            
            Creature.SetDirection(Vector2.zero);
            Particles.Spawn("Miss");
            yield return new WaitForSeconds(_missTargetCooldown);

            StartState(Patrol.DoPatrol());
        }
        
        private void AttackAndRun()
        {
            if (_attackDelay.IsReady)
            {
                Creature.Attack();
                _attackDelay.Reset();
            }
        }
    }
}