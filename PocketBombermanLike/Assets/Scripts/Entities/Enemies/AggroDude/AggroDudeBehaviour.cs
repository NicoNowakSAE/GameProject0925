using System.Linq;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the AI behavior for an aggressive enemy that toggles between 
/// mathematical figure-eight patrolling and point-based player chasing.
/// </summary>
[RequireComponent(typeof(MovementController), typeof(PointNavigation), typeof(Health))]
public class AggroDudeBehaviour : MonoBehaviour, IBombHit
{
    private Transform _transform;
    private LineOfSight _lineOfSight;
    private Rigidbody2D _rb;
    private Vector3 _startPos;
    private PointNavigation _pointNav;
    private Health _health;
    private bool _hasPointNavAlreadyStarted = false;

    private ChasingEnemyState _currentState = ChasingEnemyState.Patrolling;
    [SerializeField] private float _patrollingAreaSize;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<StateMovementspeedPair> _movementStateSpeeds = new List<StateMovementspeedPair>();
    [SerializeField] private float _playerDistanceThreshold = 0.25f;

    public void Hit(int dmg) => _health.Reduce(dmg);

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _lineOfSight = GetComponent<LineOfSight>();
        _rb = GetComponent<Rigidbody2D>();
        _startPos = _transform.position;
        _pointNav = GetComponent<PointNavigation>();
        _health = GetComponent<Health>();

        EnemyCollection.Subscribe(gameObject);

        _health.OnEntityDeath.AddListener(() => {EnemyCollection.Unsubscribe(gameObject);});
        
        _pointNav.SetNavSpeed(_movementStateSpeeds.First(pair => pair.State == ChasingEnemyState.Chasing).Speed);
    }

    /// <summary>
    /// Visualizes the detection ray towards the player in the Scene View.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_transform != null && _player != null)
        {
            Gizmos.DrawRay(
                _transform.position,
                transform.TransformDirection(_player.transform.position - _transform.position).normalized * _lineOfSight.LineOfSightLength
            );
        }
    }

    /// <summary>
    /// Calculates a figure-eight position based on time and patrolling speed, 
    /// then moves the Rigidbody2D to that position.
    /// </summary>
    private void PerformPatrollingMovement()
    {
        float t = Time.time * _movementStateSpeeds.First(pair => pair.State == ChasingEnemyState.Patrolling).Speed;

        float x = _patrollingAreaSize * Mathf.Sin(t);
        float y = _patrollingAreaSize * 0.5f * Mathf.Sin(2f * t);

        Vector3 target = _startPos + new Vector3(x, y, 0f);

        _rb.MovePosition(target);
    }

    /// <summary>
    /// Continuously feeds the player's current position into the PointNavigation system 
    /// as long as the enemy is in the Chasing state and the player has moved beyond the threshold.
    /// </summary>
    private IEnumerator TrackPlayerNavPoints()
    {
        _pointNav.SetActive(true);

        while (_currentState == ChasingEnemyState.Chasing)
        {
            float distanceToPlayer = Vector3.Distance(_transform.position, _player.transform.position);
            if (distanceToPlayer >= _playerDistanceThreshold)
            {
                _pointNav.AddPoint(_player.transform.position);
            }
            yield return new WaitForFixedUpdate();
        }

        _pointNav.SetActive(false);
    }

    /// <summary>
    /// Evaluates the current state machine. Handles transition from Patrolling to Chasing 
    /// based on Line of Sight, and initiates the player tracking coroutine.
    /// </summary>
    private void Update()
    {
        switch (_currentState)
        {
            case ChasingEnemyState.Patrolling:

                PerformPatrollingMovement();

                bool isAnyTargetInSight = _lineOfSight.CheckTargets(
                    transform.TransformDirection(
                        transform.TransformDirection(_player.transform.position - _transform.position).normalized * _lineOfSight.LineOfSightLength).normalized,
                        _transform.position
                    );

                if (isAnyTargetInSight)
                {
                    Debug.Log("[AGGRO DUDE] Found target! Starting chase now -");
                    _currentState = ChasingEnemyState.Chasing;
                }
                    
                break;
            
            case ChasingEnemyState.Chasing:
                if (!_hasPointNavAlreadyStarted)
                {
                    _hasPointNavAlreadyStarted = true;
                    Debug.Log("[AGGRO DUDE] Starting Coroutine TrackPlayerNavPoints() -");
                    StartCoroutine(TrackPlayerNavPoints());
                }
                break;

        }
    }
}