using System.Collections;
using Assets.Scripts.Component;
using Assets.Scripts.Hero;
using Assets.Scripts.UI.Widgets;
using Creatures;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] protected ProgressBarWidget _healthBar;
        [SerializeField] protected LayerCheck _vision;
        [SerializeField] protected LayerCheck _canAttack;

        [SerializeField] protected float _alarmDelay = 0.5f;
        [SerializeField] protected float _attackCooldown = 1f;
        [SerializeField] protected float _missTargetCooldown = 0.5f;
        [SerializeField] protected float _recoverCooldown = 1f;
        
        [Space]
        [Header("After Death")]
        [SerializeField] protected float _colliderSizeX = 0.1f;
        [SerializeField] protected float _colliderSizeY =  0.1f;

        protected SpawnListComponent Particles;
        protected Creature Creature;
        protected Patrol Patrol;
        protected bool IsDead;
        
        private Coroutine _current;
        private GameObject _target;
        private Animator _animator;

        private static readonly int IsDeadKey = Animator.StringToHash("IsDead");

        private void Awake()
        {
            Particles = GetComponent<SpawnListComponent>();
            Creature = GetComponent<Creature>();
            Patrol = GetComponent<Patrol>();
            _animator = GetComponent<Animator>();
        }


        private void Start()
        {
            StartState(Patrol.DoPatrol());
        }

        public void StartState(IEnumerator coroutine)
        {
            Creature.SetDirection(Vector2.zero);
            if (_current != null)
                StopCoroutine(_current);
            
            _current = StartCoroutine(coroutine);
        }

        public void OnHeroInVision(GameObject go)
        {
            if (IsDead) return;
            
            _target = go;
            StartState(AgroToTarget());
        }

        public IEnumerator OnHit()
        {
            Creature.SetDirection(Vector2.zero);
            yield return  new WaitForSeconds(_recoverCooldown);
            StartState(GoToTarget());
        }

        public void OnDie()
        {
            Creature.SetDirection(Vector2.zero);
            IsDead = true;
            _animator.SetBool(IsDeadKey, true);
            gameObject.layer = LayerMask.NameToLayer("Ground");
            _healthBar.gameObject.SetActive(false);
            
            if (_current != null)
            {
                StopCoroutine(_current);
                Creature.SetDirection(Vector2.zero);
            }
        }

        private IEnumerator AgroToTarget()
        {
            LookAtHero();
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToTarget());
        }

        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            Creature.SetDirection(Vector2.zero);
            Creature.UpdateSpriteDirection(direction);
        }

        protected virtual IEnumerator GoToTarget()
        {
            if (IsDead) yield break;

            while (_vision.IsTouchingLayer )
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return default;
            }
            
            Creature.SetDirection(Vector2.zero);
            Particles.Spawn("Miss");
            yield return new WaitForSeconds(_missTargetCooldown);

            StartState(Patrol.DoPatrol());
        }

        protected virtual IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                Creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            
            StartState(GoToTarget());
        }

        protected void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            Creature.SetDirection(direction);            
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }
    }
}
