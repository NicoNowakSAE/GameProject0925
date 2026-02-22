using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PathNavigation : MonoBehaviour
{
    [SerializeField] private List<PathNavPoint> _pathNavPoints;
    [SerializeField] private float _navMoveSpeed = 1.0f;
    [SerializeField] private float _distanceThreshold;

    private Rigidbody2D _rb;
    private Transform _transform;
    private int _currTargetPosIdx = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        if (_pathNavPoints.Count > 0)
            _transform.position = _pathNavPoints[0].Position;
    }

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
            MoveTowards(targetPos);
        }
        else
        {
            _currTargetPosIdx++;
            _rb.linearVelocity = Vector2.zero;
            _rb.position = targetPos;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - _transform.position).normalized;
        _rb.linearVelocity = dir * _navMoveSpeed;
    }
}
