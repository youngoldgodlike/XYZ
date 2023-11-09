using System.Collections;
using Component;
using UnityEngine;

namespace Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackColdown = 1f;
        [SerializeField] private float _missTargetColdown = 0.5f;
        
        [Space]
        [Header("After Death")]
        [SerializeField] private float _colliderSizeX = 0.1f;
        [SerializeField] private float _colliderSizeY =  0.1f;

        private Coroutine _current;
        private GameObject _target;
        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private Patrol _patrol;
        private CapsuleCollider2D _collider;
        private static readonly int IsDeadKey = Animator.StringToHash("IsDead");
        private bool _isDead;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            
            _target = go;
            
            StartCoroutine(AgroToTarget());
        }

        private IEnumerator AgroToTarget()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToTarget());
        }
        
        private IEnumerator GoToTarget()
        {
            while (_vision.isTouchingLayer)
            {
                if (_canAttack.isTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missTargetColdown);
        }

        private IEnumerator Attack()
        {
            while (_canAttack.isTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackColdown);
            }
            StartState(GoToTarget());
        }

        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);            
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            _collider.size = new Vector2(_colliderSizeX, _colliderSizeY);
            _collider.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Ground");
            
            if (_current != null)
            {
                StopCoroutine(_current);
                _creature.SetDirection(Vector2.zero);
            }
            
        }
    }
}
