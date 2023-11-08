using System.Collections;
using Creatures;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private Transform _followPoint;
        [SerializeField] private LayerCheck _layerChecks;

        [SerializeField] private bool _isFollowScale;

        private Creature _creature;
        private bool _onFollowScale = true;

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
                }
                
                var direction = _followPoint.position - transform.position;
                direction.y = 0;
                _creature.SetDirection(direction.normalized);
                
                yield return null;
            }
        }
    }
}
