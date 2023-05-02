using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyAi : MonoBehaviour
    {
        [SerializeField] private float roamChangeDirFloat = 2f;
        [SerializeField] private float attackRange = 0;
        [SerializeField] private MonoBehaviour enemyType;
        [SerializeField] private float attackCooldown = 1;
        [SerializeField] private bool stopMovingWhileAttacking = false;
    
        private Vector2 _roamPosition;
        private float _timeRoaming = 0;
        private State _state;
        private EnemyPathfinding _enemyPathfinding;
        private bool _canAttack = true;

        private void Awake()
        {
            _state = State.Roaming;
            _enemyPathfinding = GetComponent<EnemyPathfinding>();
        }

        private void Start()
        {
            _roamPosition = GetRoamingPosition();
        }

        private void Update()
        {
            MovementStateControl();
        }

        private void MovementStateControl()
        {
            switch (_state)
            {
                case State.Roaming:
                    Roaming();
                    break;
                case State.Attacking:
                    Attacking();
                    break;
                default:
                    break;
            }
        }

        private void Roaming()
        {
            _timeRoaming += Time.deltaTime; 
        
            _enemyPathfinding.MoveTo(_roamPosition);

            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackRange)
            {
                _state = State.Attacking;
            }

            if (_timeRoaming > roamChangeDirFloat)
            {
                _roamPosition = GetRoamingPosition();
            }
        }

        private void Attacking()
        {
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
            {
                _state = State.Roaming;
            }
            
            if (attackRange == 0 || !_canAttack) return;
            
            ((IEnemy)enemyType).Attack();
            _canAttack = false;

            if (stopMovingWhileAttacking)
            {
                _enemyPathfinding.StopMoving();
            }
            else
            {
                _enemyPathfinding.MoveTo(_roamPosition);
            }
            
            StartCoroutine(AttackCooldownRoutine());
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
    
        private Vector2 GetRoamingPosition()
        {
            _timeRoaming = 0;
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        private enum State
        {
            Roaming,
            Attacking,
        }
    }
}
