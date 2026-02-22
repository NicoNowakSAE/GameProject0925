using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PointNavigation : MonoBehaviour
{
    [SerializeField] private float _minPointSpacing = 5.0f;
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

    private void RemoveOldestPoint()
    {
        if (_points.Count <= 0)
            return;

        _points.RemoveAt(0);
    }

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

        Debug.Log($"[POINT NAVIGATION] Set active: {value} -");
    }

    public void SetNavSpeed(float speed)
    {
        _navMoveSpeed = speed;
        Debug.Log($"[POINT NAVIGATION] Set nav speed: {_navMoveSpeed} -");
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - _transform.position).normalized;
        _rb.linearVelocity = direction * _navMoveSpeed;
    }

    private IEnumerator MovementLoop()
    {
        _isMovementLoopAlreadyActive = true;
        while (_isActive)
        {
            Debug.Log($"[POINT NAVIGATION] Array Size: {_points.Count} -");

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
