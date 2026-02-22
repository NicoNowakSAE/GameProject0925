using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles point-to-point navigation for a 2D object using a Rigidbody2D.
/// Supports a dynamic list of points with distance-based thresholds.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PointNavigation : MonoBehaviour
{
    [Tooltip("The minimum space between each tracked point.")]
    [SerializeField] private float _minPointSpacing = 5.0f;

    [Tooltip("The maximum amount of points tracked.")]
    [SerializeField] private int _maxPointArraySize = 50;

    private List<Vector3> _points = new List<Vector3>();
    private Rigidbody2D _rb;
    private Transform _transform;
    private bool _isActive = false;
    private float _navMoveSpeed = 3.0f;
    private bool _isMovementLoopAlreadyActive = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    /// <summary>
    /// Adds a new destination point to the navigation list if it meets spacing and capacity requirements.
    /// </summary>
    /// <param name="point">The world space position to add.</param>
    public void AddPoint(Vector3 point)
    {
        if (_points.Count >= _maxPointArraySize)
            RemoveOldestPoint();

        if (_points.Count > 1)
        {
            if ((_points[_points.Count - 1] - point).magnitude >= _minPointSpacing)
            {
                _points.Add(point);
            }
        }
        else
        {
            _points.Add(point);
        }
    }

    /// <summary>
    /// Removes the first element (index 0) from the points list.
    /// </summary>
    private void RemoveOldestPoint()
    {
        if (_points.Count <= 0)
            return;

        _points.RemoveAt(0);
    }

    /// <summary>
    /// Toggles the navigation movement on or off and manages the MovementLoop coroutine.
    /// </summary>
    /// <param name="value">True to enable movement, false to disable.</param>
    public void SetActive(bool value)
    {
        _isActive = value;

        if (value == false)
        {
            StopCoroutine(MovementLoop());
        }

        if (value == true && !_isMovementLoopAlreadyActive)
        {
            StartCoroutine(MovementLoop());
        }
    }

    /// <summary>
    /// Updates the movement speed used during navigation.
    /// </summary>
    /// <param name="speed">The new move speed value.</param>
    public void SetNavSpeed(float speed)
    {
        _navMoveSpeed = speed;
    }

    /// <summary>
    /// Calculates direction and applies linear velocity to the Rigidbody2D toward a target.
    /// </summary>
    /// <param name="target">The destination position.</param>
    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - _transform.position).normalized;
        _rb.linearVelocity = direction * _navMoveSpeed;
    }

    /// <summary>
    /// The core logic loop that processes points in the list and moves the object toward them sequentially.
    /// </summary>
    private IEnumerator MovementLoop()
    {
        _isMovementLoopAlreadyActive = true;
        while (_isActive)
        {
            if (_points.Count == 0)
            {
                yield return null;
                continue;
            }

            while (_points.Count > 0 && Vector3.Distance(_transform.position, _points[0]) > 0.25f)
            {
                MoveTowards(_points[0]);
                yield return new WaitForFixedUpdate();
            }

            RemoveOldestPoint();

        }
        _isMovementLoopAlreadyActive = false;
    }

    /// <summary>
    /// Visualizes the navigation path in the Unity Editor Scene View using spheres and lines.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_points == null || _points.Count == 0)
            return;

        Vector3[] tempList = _points.ToArray();

        for (int i = 0; i < tempList.Count(); i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(tempList[i], 0.1f);

            if (i < tempList.Count() - 1)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(tempList[i], _points[i + 1]);
            }
        }
    }
}