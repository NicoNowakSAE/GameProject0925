using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Handles 2D movement and facing direction for an entity.
/// Provides constant horizontal movement and flip functionality.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    /// <summary>
    /// Base movement speed used as default input for movement.
    /// </summary>
    [SerializeField] private float _baseSpeed = 3.0f;

    /// <summary>
    /// Public read-only access to the base movement speed.
    /// </summary>
    public float BaseSpeed => _baseSpeed;

    /// <summary>
    /// Rigidbody used to apply movement.
    /// </summary>
    private Rigidbody2D _rb;

    /// <summary>
    /// Cached transform reference for scale manipulation.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Current horizontal movement direction.
    /// Right is default, left after flip.
    /// </summary>
    public Vector2 MoveDirection { get; private set; } = Vector2.right;

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    /// <summary>
    /// Applies horizontal movement using the given speed
    /// while preserving the current vertical velocity.
    /// </summary>
    /// <param name="speed">Movement speed applied in move direction.</param>
    public void Move(float speed)
    {
        _rb.linearVelocity = new Vector2(
            speed * MoveDirection.x,
            _rb.linearVelocityY
        );
    }

    /// <summary>
    /// Flips the facing direction by inverting local scale
    /// and reversing the movement direction.
    /// </summary>
    public void Flip()
    {
        Vector3 currLocalScale = _transform.localScale;
        _transform.localScale = new Vector3(
            currLocalScale.x * -1,
            currLocalScale.y,
            currLocalScale.z
        );

        MoveDirection *= -1;
    }

    /// <summary>
    /// Removes the entire linear velocity of the targeted Rigidbody.
    /// </summary>
    public void StopMovement()
    {
        _rb.linearVelocity = Vector3.zero;
    }
}
