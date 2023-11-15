using Assets.Scripts.Component;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class TotemHeadAI : MonoBehaviour
    {
        [SerializeField] private SpawnComponent _rangeProjectile;
        
        private Animator _animator;
        private static readonly int IsRange = Animator.StringToHash("IsRange");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        [ContextMenu("Attack")]
        public void Attack()
        {
            _animator.SetTrigger(IsRange);
        }

        public void OnAttack()
        {
            _rangeProjectile.Spawn();
        }
        
        



    }
}
