using Assets.Scripts.Component;
using Assets.Scripts.Hero;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;
        [SerializeField] private Cooldown _meleeCooldown;
        
        [Header("Range")]
        [SerializeField] private SpawnComponent _rangeAttack;
        [SerializeField] private Cooldown _rangeCooldown;
        
        private Animator _animator;
        private static readonly int IsMelee = Animator.StringToHash("IsMelee");
        private static readonly int IsRange = Animator.StringToHash("IsRange");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady)
                        MeleeAttack();
                    return;
                }
                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(IsMelee);
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(IsRange);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }
        
        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
        
    }
}