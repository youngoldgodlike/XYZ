using System.Collections;
using Creatures;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private Transform _followPoint;
        [SerializeField] private LayerCheck _layerChecks;
        private Creature _creature;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }
        
        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (!_layerChecks.isTouchingLayer)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, 1,1);
                    _creature.SetDirection(new Vector2(0,0));
                    yield return new WaitForSeconds(1f);
                    
                }
                
                var direction = _followPoint.position - transform.position;
                direction.y = 0;
                _creature.SetDirection(direction.normalized);
                
                yield return null;
            }
        }
    }
}
