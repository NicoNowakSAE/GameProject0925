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
        Debug.Log($"[POINT NAVIGATION] Set active: {value} -");
    }

    public void SetNavSpeed(float speed)
    {
        _navMoveSpeed = speed;
        Debug.Log($"[POINT NAVIGATION] Set nav speed: {_navMoveSpeed} -");
    }

    private void MoveToNextPoint()
    {
        if (_points.Count <= 0)
            return;

        Vector3 targetPoint = _points.ElementAt(0);
        Vector3 direction = transform.TransformDirection(targetPoint - _transform.position);

        Debug.Log($"[POINT NAVIGTION] Moving to point: {targetPoint} -");
        _rb.linearVelocity = direction * _navMoveSpeed;
    }

    private IEnumerator MovementLoop()
    {
        _isMovementLoopAlreadyActive = true;
        while (_isActive)
        {
            while (Vector3.Distance(_transform.position, _points.ElementAt(0)) > 0.25f)
            {
                MoveToNextPoint();
            }
            
            RemoveOldestPoint();
            yield return new WaitForEndOfFrame();
        }
        _isMovementLoopAlreadyActive = false;
    }

    private void Update()
    {
        if (_isActive && !_isMovementLoopAlreadyActive)
            StartCoroutine(MovementLoop());

        Debug.Log($"Array Size: {_points.Count}\nIs Active: {_isActive} -");
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up * 3,
            $"Points: {_points.Count}");

        if (_points == null || _points.Count == 0)
            return;

        Vector3[] tempList = _points.ToArray();

        for (int i = 0; i < tempList.Count(); i++)
        {
            // Punkt zeichnen
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(tempList[i], 0.2f);

            if (i < tempList.Count() - 1)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(tempList[i], _points[i + 1]);
            }
        }
    }
}
