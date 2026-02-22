using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controls an object's movement along a predefined path of PathNavPoint objects.
/// Uses Rigidbody2D for movement and includes support for distance thresholds and per-point cooldowns.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PathNavigation : MonoBehaviour
{
    [Header("Path Points")]

    [Tooltip("A collection of all path target points this object should travel to.")]
    [SerializeField] private List<PathNavPoint> _pathNavPoints;

    [Header("Move Speed")]

    [Tooltip("Determines the movement speed of the navigating object.")]
    [SerializeField] private float _navMoveSpeed = 1.0f;

    [Header("Navigation Settings")]

    [Tooltip("Determines the theshold below which the platform marks the target position as reached.")]
    [SerializeField] private float _distanceThreshold;

    [Tooltip("Cooldown after each reached path point.")]
    [SerializeField] private float _targetCooldownTime = 0.0f;

    private float _currCooldownTime = 0.0f;
    private Rigidbody2D _rb;
    private Transform _transform;
    private int _currTargetPosIdx = 0;
    private Vector2 _lastFrameDelta;
    public Vector2 LastFrameDelta => _lastFrameDelta;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    /// <summary>
    /// Snaps the object to the first path point (if available) and sets the initial target index.
    /// </summary>
    private void Start()
    {
        if (_pathNavPoints.Count > 0)
            _transform.position = _pathNavPoints[0].Position;

        _currTargetPosIdx = 1;
    }

    /// <summary>
    /// Handles the physics-based movement logic, distance checking, and path indexing.
    /// </summary>
    private void FixedUpdate()
    {
        if (_currTargetPosIdx > _pathNavPoints.Count - 1)
        {
            _currTargetPosIdx = 0;
        }

        Vector3 targetPos = _pathNavPoints[_currTargetPosIdx].Position;
        float distance = (_transform.position - targetPos).sqrMagnitude;

        if (distance > (_distanceThreshold * _distanceThreshold))
        {
            if (_currCooldownTime <= 0.0f)
                MoveTowards(targetPos);
        }
        else
        {
            _currTargetPosIdx++;
            _rb.linearVelocity = Vector2.zero;
            _rb.position = targetPos;
            _lastFrameDelta = Vector3.zero;

            _currCooldownTime = _targetCooldownTime;
        }
    }

    /// <summary>
    /// Updates the internal cooldown timer using frame-independent time.
    /// </summary>
    private void Update()
    {
        _currCooldownTime -= Time.deltaTime;
    }

    /// <summary>
    /// Calculates the direction to the target and applies velocity to the Rigidbody2D.
    /// </summary>
    /// <param name="target">The world space position to move toward.</param>
    private void MoveTowards(Vector3 target)
    {
        Vector2 currentPos = _rb.position;
        Vector2 targetPos = (Vector2)target;
        Vector2 dir = (targetPos - currentPos).normalized;

        Vector2 step = dir * _navMoveSpeed * Time.fixedDeltaTime;

        _lastFrameDelta = step;

        _rb.MovePosition(currentPos + step);
    }

    private void OnDrawGizmos()
    {
        if (_pathNavPoints.Count >= 2)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLineStrip(_pathNavPoints.Select(point => point.Position).ToArray(), true);
        }
    }
}