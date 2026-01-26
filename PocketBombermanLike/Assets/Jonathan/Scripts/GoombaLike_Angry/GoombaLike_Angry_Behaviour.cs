using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoombaLike_Angry_Behaviour : MonoBehaviour
{
    [Header("Checkbox Properties")]
    [SerializeField] private Transform _wallCheckPosition;
    [SerializeField] private Area _wallCheckBoxDimensions;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private Area _groundCheckBoxDimensions;

    [Header("Layers To Check")]
    [SerializeField] private LayerMask _layersToCheck;

    [Header("Movement Properties")]
    [SerializeField] private float _moveSpeed = 2.0f;

    [Header("Line Of Sight Properties")]
    [SerializeField] private float _lineOfSightLength = 2.0f;
    [SerializeField] private LayerMask _lineOfSightTargetLayers;
    [SerializeField] private float _chaseModeSpeed = 5.0f;

    private EnemyState _currentState = EnemyState.Idle;
    private Vector2 _moveDirection = Vector2.right;
    private Transform _transform;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        Debug.Log("[ANGRY GOOMBA] initialized -");
    }

    private bool IsWallAhead()
    {
        Collider2D overlapCollider = Physics2D.OverlapBox(
            new Vector2(
                _wallCheckPosition.position.x,
                _wallCheckPosition.position.y
            ),
            new Vector2(
                _wallCheckBoxDimensions.Width,
                _wallCheckBoxDimensions.Height
            ),
            0.0f,
            _layersToCheck
        );

        return overlapCollider != null;
    }

    private bool IsGrounded()
    {
        Collider2D overlapCollider = Physics2D.OverlapBox(
            new Vector2(
                _groundCheckPosition.position.x,
                _groundCheckPosition.position.y
            ),
            new Vector2(
                _groundCheckBoxDimensions.Width,
                _groundCheckBoxDimensions.Height
            ),
            0.0f,
            _layersToCheck
        );

        return overlapCollider != null;
    }

    private void FlipEntity()
    {
        _transform.localScale = new Vector3(
            _transform.localScale.x * -1,
            _transform.localScale.y,
            _transform.localScale.z
        );

        _moveDirection *= -1;

        Debug.Log("[ANGRY GOOMBA] flipping direction -");
    }

    private void Move()
    {
        float speed;

        switch (_currentState)
        {
            case EnemyState.Chasing:
                speed = _chaseModeSpeed;
                break;

            case EnemyState.Idle:
                speed = _moveSpeed;
                break;

            default:
                speed = _moveSpeed;
                break;
        }

        _rb.linearVelocity = new Vector2(
            speed * _moveDirection.x,
            _rb.linearVelocity.y
        );
    }

    private void ToggleState(EnemyState targetState)
    {
        _currentState = targetState;

        switch (targetState)
        {
            case EnemyState.Chasing:
                Debug.Log("[ANGRY GOOMBA] entering chase mode -");
                break;

            case EnemyState.Idle:
                Debug.Log("[ANGRY GOOMBA] returning to idle mode -");
                break;
        }
    }

    private bool CheckForTargetsInLineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            _transform.position,
            _moveDirection,
            _lineOfSightLength,
            _lineOfSightTargetLayers
        );

        return hit.collider != null;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        bool isWallAhead = IsWallAhead();
        bool isGrounded = IsGrounded();
        bool isTargetInLineOfSight = CheckForTargetsInLineOfSight();

        if (isWallAhead)
        {
            Debug.Log("[ANGRY GOOMBA] wall ahead; flipping sprite -");
        }
        else if (!isGrounded)
        {
            Debug.Log("[ANGRY GOOMBA] no ground ahead; flipping sprite -");
        }

        if (isTargetInLineOfSight)
        {
            if (_currentState != EnemyState.Chasing)
            {
                Debug.Log("[ANGRY GOOMBA] target spotted; starting chase -");
                ToggleState(EnemyState.Chasing);
            }
        }
        else
        {
            if (_currentState == EnemyState.Chasing)
            {
                Debug.Log("[ANGRY GOOMBA] lost target; stopping chase -");
                ToggleState(EnemyState.Idle);
            }
        }

        if (isWallAhead || !isGrounded)
        {
            FlipEntity();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(
            new Vector3(
                _wallCheckPosition.position.x,
                _wallCheckPosition.position.y,
                0.0f
            ),
            new Vector3(
                _wallCheckBoxDimensions.Width,
                _wallCheckBoxDimensions.Height,
                0.0f
            )
        );

        Gizmos.DrawWireCube(
            new Vector3(
                _groundCheckPosition.position.x,
                _groundCheckPosition.position.y,
                0.0f
            ),
            new Vector3(
                _groundCheckBoxDimensions.Width,
                _groundCheckBoxDimensions.Height,
                0.0f
            )
        );

        Gizmos.color = Color.yellow;

        if (_transform != null)
        {
            Gizmos.DrawLine(
                _transform.position,
                _transform.position + new Vector3(_moveDirection.x, _moveDirection.y, 0) * _lineOfSightLength
            );
        }
    }
}
