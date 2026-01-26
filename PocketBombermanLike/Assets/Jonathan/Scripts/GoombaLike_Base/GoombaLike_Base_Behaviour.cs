using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoombaLike_Base_Behaviour : MonoBehaviour
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

    private Vector2 _moveDirection = Vector2.right;
    private Transform _transform;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        Debug.Log("[GOOMBA] initialized -");
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

        Debug.Log("[GOOMBA] flipping direction -");
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector2(
            _moveSpeed * _moveDirection.x,
            _rb.linearVelocity.y
        );
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        bool isWallAhead = IsWallAhead();
        bool isGrounded = IsGrounded();

        if (isWallAhead)
        {
            Debug.Log("[GOOMBA] wall ahead; flipping sprite -");
        }
        else if (!isGrounded)
        {
            Debug.Log("[GOOMBA] no ground ahead; flipping sprite -");
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
    }
}
