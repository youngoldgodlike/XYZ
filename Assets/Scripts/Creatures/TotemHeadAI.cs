using Assets.Scripts.Component;
using Assets.Scripts.Component.Audio;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class TotemHeadAI : MonoBehaviour
    {
        [SerializeField] private SpawnComponent _rangeProjectile;
        
        private Animator _animator;
        private PlaySoundsComponent _sounds;
        private static readonly int IsRange = Animator.StringToHash("IsRange");

        private void Awake()
        {
            _sounds = GetComponent<PlaySoundsComponent>();
            _animator = GetComponent<Animator>();
        }

        [ContextMenu("Attack")]
        public void Attack()
        {
            _sounds.Play("Attack");
            _animator.SetTrigger(IsRange);
        }

        public void OnAttack()
        {
            _rangeProjectile.Spawn();
        }
        
        



    }
}
