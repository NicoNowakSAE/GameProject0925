using System.Linq;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MovementController), typeof(PointNavigation))]
public class AggroDudeBehaviour : MonoBehaviour
{
    private Transform _transform;
    private LineOfSight _lineOfSight;
    private Rigidbody2D _rb;
    private Vector3 _startPos;
    private PointNavigation _pointNav;
    private bool _hasPointNavAlreadyStarted = false;

    private ChasingEnemyState _currentState = ChasingEnemyState.Patrolling;
    [SerializeField] private float _patrollingAreaSize;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<StateMovementspeedPair> _movementStateSpeeds = new List<StateMovementspeedPair>();
    [SerializeField] private float _playerDistanceThreshold = 0.25f;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _lineOfSight = GetComponent<LineOfSight>();
        _rb = GetComponent<Rigidbody2D>();
        _startPos = _transform.position;
        _pointNav = GetComponent<PointNavigation>();

        _pointNav.SetNavSpeed(_movementStateSpeeds.First(pair => pair.State == ChasingEnemyState.Chasing).Speed);
    }

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

    private void PerformPatrollingMovement()
    {
        float t = Time.time * _movementStateSpeeds.First(pair => pair.State == ChasingEnemyState.Patrolling).Speed;

        float x = _patrollingAreaSize * Mathf.Sin(t);
        float y = _patrollingAreaSize * 0.5f * Mathf.Sin(2f * t);

        Vector3 target = _startPos + new Vector3(x, y, 0f);

        _rb.MovePosition(target);
    }

    private IEnumerator TrackPlayerNavPoints()
    {
        _pointNav.SetActive(true);

        while (_currentState == ChasingEnemyState.Chasing)
        {
            float distanceToPlayer = Vector3.Distance(_transform.position, _player.transform.position);
            if (distanceToPlayer >= _playerDistanceThreshold)
            {
                _pointNav.AddPoint(_player.transform.position);
                Debug.Log($"[AGGRO DUDE] Adding new point to point nav: {_player.transform.position} -");
            }
            yield return new WaitForFixedUpdate();
        }

        _pointNav.SetActive(false);
    }

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
