using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Follow")]
    [SerializeField] private bool _followTarget = true;
    [SerializeField] private Transform _targetTransform;
    private Rigidbody2D _targetRb;
    [SerializeField] private Vector3 _trackingOffset = Vector3.zero;

    [Header("Tracking Behaviour")]
    [SerializeField][Range(0.0f, 9.0f)] private float _positionTrackingSmoothness;
    [SerializeField][Range(0.0f, 10.0f)] private float _horizontalDirectionOffset;
    [SerializeField][Range(0.0f, 9.0f)] private float _horizontalDirectionSmoothness;
    [SerializeField] private bool _changePovBasedOnHeight;

    private Transform _cameraTransform;
    private float _maxPositionTrackingSmoothness = 10.0f;
    private float _maxHorizontalDirectionSmoothness = 10.0f;
    private float _linearVelocityThreshold = 0.5f;

    private void Awake()
    {
        Camera camera = Camera.main;

        if (camera == null)
            Debug.LogWarning("[CAMERA CONTROLLER] No main camera found -");

        _cameraTransform = camera.GetComponent<Transform>();

        _targetRb = _targetTransform.gameObject.GetComponent<Rigidbody2D>();

        _cameraTransform.position = _targetRb.position + (Vector2)_trackingOffset;
    }

    private void Update()
    {
        if (_followTarget && _targetTransform)
        {
            Vector3 _targetPos = _targetTransform.position + _trackingOffset;
            Vector3 _currPos = _cameraTransform.position;
            float horizontalOffset = 0.0f;

            if (_targetRb.linearVelocity.x > _linearVelocityThreshold)
            {
                horizontalOffset = Mathf.Lerp(
                    _currPos.x,
                    _targetPos.x + _horizontalDirectionOffset,
                    Time.deltaTime * (_maxHorizontalDirectionSmoothness - _horizontalDirectionSmoothness)
                );

                _targetPos.x = horizontalOffset;
            }
            else if (_targetRb.linearVelocity.x < _linearVelocityThreshold * -1)
            {
                horizontalOffset = Mathf.Lerp(
                    _currPos.x,
                    _targetPos.x - _horizontalDirectionOffset,
                    Time.deltaTime * (_maxHorizontalDirectionSmoothness - _horizontalDirectionSmoothness)
                );

                _targetPos.x = horizontalOffset;
            }
            else
            {
                horizontalOffset = Mathf.Lerp(
                    _currPos.x,
                    _targetPos.x,
                    Time.deltaTime * (_maxHorizontalDirectionSmoothness - _horizontalDirectionSmoothness)
                );

                _targetPos.x = horizontalOffset;
            }

            _targetPos = Vector3.LerpUnclamped(
                    new Vector3(_targetPos.x, _currPos.y, _trackingOffset.z),
                    _targetPos,
                    (_maxPositionTrackingSmoothness - _positionTrackingSmoothness) * Time.deltaTime
            );

            SetCameraPosition(_targetPos);
        }

    }

    public void SetCameraPosition(Vector3 position)
    {
        _cameraTransform.position = position;
    }

}
