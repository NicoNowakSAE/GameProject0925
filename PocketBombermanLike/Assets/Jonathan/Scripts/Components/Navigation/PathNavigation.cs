using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PathNavigation : MonoBehaviour
{
    [SerializeField] private List<PathNavPoint> _pathNavPoints;
    [SerializeField] private float _navMoveSpeed = 1.0f;
    [SerializeField] private float _timeoutAfterPoints;
    private Rigidbody2D _rb;
    private Transform _transform;
    private int _currTargetPosIdx = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        Debug.Log($"[PATH NAVIGATION] Found {_pathNavPoints.Count} points to navigate -");
    }

    private void Start()
    {
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
            if (_currTargetPosIdx > _pathNavPoints.Count - 1)
                _currTargetPosIdx = 0;

            Vector3 targetPoint = _pathNavPoints[_currTargetPosIdx].Position;
            Vector2 dir = transform.TransformDirection(targetPoint - _transform.position);
            float sqrDistanceToNextPoint = (targetPoint - _transform.position).sqrMagnitude;

            while (sqrDistanceToNextPoint > 20 && _pathNavPoints.Count > 1)
                MoveTowards(targetPoint);

            _currTargetPosIdx++;

            yield return new WaitForSeconds(_timeoutAfterPoints);
        }
    }
}
