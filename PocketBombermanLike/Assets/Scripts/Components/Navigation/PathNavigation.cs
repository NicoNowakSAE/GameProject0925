using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controls an object's movement along a predefined path of PathNavPoint objects.
/// </summary>
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
    private Transform _transform;
    private int _currTargetPosIdx = 0;

    private void Awake()
    {
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
    /// Updates the internal cooldown timer using frame-independent time.
    /// </summary>
    private void Update()
    {
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

            if (_currTargetPosIdx > _pathNavPoints.Count - 1)
            {
                _currTargetPosIdx = 0;
            }

            _transform.position = targetPos;
            _currCooldownTime = _targetCooldownTime;
        }

        _currCooldownTime -= Time.deltaTime;
    }

    /// <summary>
    /// Calculates the direction to the target and applies velocity to the Rigidbody2D.
    /// </summary>
    /// <param name="target">The world space position to move toward.</param>
    private void MoveTowards(Vector3 target)
    {
        Vector2 currentPos = _transform.position;
        Vector2 targetPos = (Vector2)target;
        Vector2 dir = (targetPos - currentPos).normalized;

        Vector2 step = dir * _navMoveSpeed * Time.deltaTime;

        _transform.position += (Vector3)step;
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