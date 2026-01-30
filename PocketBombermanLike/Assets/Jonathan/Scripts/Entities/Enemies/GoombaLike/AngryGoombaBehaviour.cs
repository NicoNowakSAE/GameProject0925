using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class AngryGoombaBehaviour : EntityBaseBehaviour
{
    #region Serialized Fields

    [Header("Movement Settings")]
    /// <summary>
    /// Multiplier applied to base movement speed
    /// when a target is detected in line of sight.
    /// </summary>
    [SerializeField] private float _targetInSightSpeedMultiplier = 1.5f;

    [Header("Chase Settings")]
    /// <summary>
    /// Duration the Goomba actively chases a detected target.
    /// </summary>
    [SerializeField] private float _chaseDuration;

    /// <summary>
    /// Duration the Goomba remains exhausted after chasing.
    /// </summary>
    [SerializeField] private float _exhaustedDuration;

    [Header("State Settings")]
    /// <summary>
    /// Current state of the Goomba (Patrolling, Chasing, Exhausted).
    /// </summary>
    [SerializeField] private ChasingEnemyState _currentState = ChasingEnemyState.Patrolling;

    #endregion

    #region Components / References

    /// <summary>
    /// Handles line-of-sight checks for detecting targets.
    /// </summary>
    private LineOfSight _lineOfSight;

    /// <summary>
    /// Controls horizontal movement and direction.
    /// </summary>
    private MovementController _movementController;

    /// <summary>
    /// Provides environmental checks like ground and wall detection.
    /// </summary>
    private Sensor _sensor;

    /// <summary>
    /// Cached transform reference for position access.
    /// </summary>
    private Transform _transform;

    #endregion

    #region State Variables

    /// <summary>
    /// True if a target is currently detected in line of sight.
    /// </summary>
    private bool _isTargetInLineOfSight = false;

    #endregion

    #region Events

    [Header("Events")]
    /// <summary>
    /// Invoked when the Goomba starts chasing a detected target.
    /// </summary>
    public UnityEvent OnChaseStart;

    /// <summary>
    /// Invoked when the Goomba stops chasing a target.
    /// </summary>
    public UnityEvent OnChaseEnd;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _lineOfSight = GetComponent<LineOfSight>();
        _movementController = GetComponent<MovementController>();
        _sensor = GetComponent<Sensor>();
        _transform = transform;
    }

    private void OnDrawGizmos()
    {
        if (_transform == null || _movementController == null || _lineOfSight == null)
            return;

        Gizmos.color = _isTargetInLineOfSight ? Color.red : Color.green;

        Gizmos.DrawLine(
            _transform.position,
            _transform.position + new Vector3(
                _movementController.MoveDirection.x,
                _movementController.MoveDirection.y,
                0f
            ) * _lineOfSight.LineOfSightLength
        );
    }

    #endregion

    #region Core Behaviour Methods

    /// <summary>
    /// Handles physics-based movement.
    /// Adjusts speed based on line-of-sight target detection and current state.
    /// </summary>
    public override void FixedTick()
    {
        if (_currentState == ChasingEnemyState.Exhausted)
        {
            _movementController.StopMovement();
            Debug.Log("[ANGRY GOOMBA] FixedTick: Currently exhausted, stopping movement -");
            return;
        }

        _isTargetInLineOfSight = _lineOfSight.CheckTargets(
            _movementController.MoveDirection,
            _transform.position
        );

        float targetSpeed = _movementController.BaseSpeed;

        if (_currentState == ChasingEnemyState.Chasing)
        {
            targetSpeed *= _targetInSightSpeedMultiplier;
            Debug.Log($"[ANGRY GOOMBA] FixedTick: Target in sight, applying speed multiplier {_targetInSightSpeedMultiplier} -");
        }

        if (_isTargetInLineOfSight && _currentState == ChasingEnemyState.Patrolling)
        {
            Debug.Log("[ANGRY GOOMBA] FixedTick: Target detected, starting chase cycle -");
            StartCoroutine(PerformChaseCycle());
        }

        if (_currentState != ChasingEnemyState.Exhausted)
        {
            _movementController.Move(targetSpeed);
        }
    }

    /// <summary>
    /// Handles non-physics logic.
    /// Flips movement direction when hitting a wall or reaching an edge.
    /// </summary>
    public override void Tick()
    {
        if (!_sensor.IsGrounded() || _sensor.IsWallAhead())
        {
            _movementController.Flip();
        }
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Coroutine to handle chasing and exhaustion cycle.
    /// </summary>
    private IEnumerator PerformChaseCycle()
    {
        Debug.Log("[ANGRY GOOMBA] PerformChaseCycle: Starting coroutine -");

        _currentState = ChasingEnemyState.Chasing;
        OnChaseStart?.Invoke();
        yield return new WaitForSeconds(_chaseDuration);

        _currentState = ChasingEnemyState.Exhausted;
        yield return new WaitForSeconds(_exhaustedDuration);

        _currentState = ChasingEnemyState.Patrolling;
        OnChaseEnd?.Invoke();

        Debug.Log("[ANGRY GOOMBA] PerformChaseCycle: Coroutine ended -");
    }

    #endregion
}
