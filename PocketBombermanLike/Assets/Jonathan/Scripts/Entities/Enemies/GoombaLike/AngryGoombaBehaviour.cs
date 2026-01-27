using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Implements an aggressive Goomba-like behaviour.
/// Increases movement speed when a target is detected in line of sight
/// and flips direction on walls or edges.
/// </summary>
public class AngryGoombaBehaviour : MonoBehaviour, IGoombaLikeBehaviour
{
    /// <summary>
    /// Multiplier applied to base movement speed
    /// when a target is detected in line of sight.
    /// </summary>
    [SerializeField] private float _targetInSightSpeedMultiplier = 1.5f;

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
    private Transform _cachedTransform;

    /// <summary>
    /// True if a target is currently detected in line of sight.
    /// </summary>
    private bool _isTargetInLineOfSight = false;

    /// <summary>
    /// Indicates whether the Goomba is currently in a chasing state.
    /// </summary>
    private bool _isChasing;

    /// <summary>
    /// Returns whether the Goomba is currently chasing a target.
    /// </summary>
    public bool IsChasing => _isChasing;

    /// <summary>
    /// Invoked when the Goomba starts chasing a detected target.
    /// </summary>
    public UnityEvent OnChaseStart;

    /// <summary>
    /// Invoked when the Goomba stops chasing a target.
    /// </summary>
    public UnityEvent OnChaseEnd;

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _lineOfSight = GetComponent<LineOfSight>();
        _movementController = GetComponent<MovementController>();
        _sensor = GetComponent<Sensor>();
        _cachedTransform = transform;
    }

    /// <summary>
    /// Handles physics-based movement.
    /// Adjusts speed based on line-of-sight target detection.
    /// </summary>
    public void FixedTick()
    {
        _isTargetInLineOfSight = _lineOfSight.CheckTargets(
            _movementController.MoveDirection,
            _cachedTransform.position
        );

        float targetSpeed = _movementController.BaseSpeed;

        if (_isTargetInLineOfSight)
        {
            if (!_isChasing)
                _isChasing = true;
            OnChaseStart?.Invoke();

            targetSpeed *= _targetInSightSpeedMultiplier;
        }
        else
        {
            if (_isChasing)
            {
                _isChasing = false;
                OnChaseEnd?.Invoke();
            }
        }

        _movementController.Move(targetSpeed);
    }

    /// <summary>
    /// Handles non-physics logic.
    /// Flips movement direction when hitting a wall or reaching an edge.
    /// </summary>
    public void Tick()
    {
        if (!_sensor.IsGrounded() || _sensor.IsWallAhead())
        {
            _movementController.Flip();
        }
    }

    /// <summary>
    /// Draws gizmos to visualize line of sight and detection state.
    /// Red indicates a detected target, green indicates no target.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_cachedTransform == null || _movementController == null || _lineOfSight == null)
            return;

        Gizmos.color = _isTargetInLineOfSight ? Color.red : Color.green;

        Gizmos.DrawLine(
            _cachedTransform.position,
            _cachedTransform.position + new Vector3(
                _movementController.MoveDirection.x,
                _movementController.MoveDirection.y,
                0f
            ) * _lineOfSight.LineOfSightLength
        );
    }
}
