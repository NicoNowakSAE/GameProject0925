using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PathNavigation : MonoBehaviour
{
    [SerializeField] private List<PathNavPoint> _pathNavPoints;
    [SerializeField] private float _navMoveSpeed = 1.0f;
    [SerializeField] private float _timeoutAfterPoints;
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

        StartCoroutine(MovementLoop());
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - _rb.position).normalized;
        _rb.linearVelocity = dir * _navMoveSpeed;
    }

    private IEnumerator MovementLoop()
    {
        _currTargetPosIdx = 0;

        while (true)
        {
            if (_pathNavPoints.Count == 0)
            {
                yield return null;
                continue;
            }

            if (_currTargetPosIdx >= _pathNavPoints.Count - 1)
                _currTargetPosIdx = 0;

            Vector3 targetPoint = _pathNavPoints[_currTargetPosIdx].Position;

            while (Vector3.Distance(targetPoint, _rb.position) > 2.0f)
            {
                MoveTowards(targetPoint);
                yield return new WaitForFixedUpdate();
            }

            Debug.Log(_currTargetPosIdx);
            Debug.Log(targetPoint);
            Debug.Log("reached target.");
            _currTargetPosIdx++;

            yield return new WaitForSeconds(_timeoutAfterPoints);
        }
    }
}
