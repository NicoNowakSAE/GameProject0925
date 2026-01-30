using Mono.Cecil;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Provides environmental sensing for an entity.
/// Detects ground and walls using overlap box checks.
/// </summary>
public class Sensor : MonoBehaviour
{
    /// <summary>
    /// World position used as the center of the wall detection box.
    /// </summary>
    [Header("Checkbox Properties")]
    [SerializeField] private Transform _wallCheckPosition;

    /// <summary>
    /// Dimensions of the wall detection box.
    /// </summary>
    [SerializeField] private Area _wallCheckBoxDimensions;

    /// <summary>
    /// World position used as the center of the ground detection box.
    /// </summary>
    [SerializeField] private Transform _groundCheckPosition;

    /// <summary>
    /// Dimensions of the ground detection box.
    /// </summary>
    [SerializeField] private Area _groundCheckBoxDimensions;

    /// <summary>
    /// Layers considered for ground and wall detection.
    /// </summary>
    [Header("Layers To Check")]
    [SerializeField] private LayerMask _layersToCheck;

    /// <summary>
    /// Checks if a wall is detected in front of the entity.
    /// </summary>
    /// <returns>
    /// True if a collider is detected in the wall check box,
    /// otherwise false.
    /// </returns>
    public bool IsWallAhead()
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

    /// <summary>
    /// Checks if the entity is grounded.
    /// </summary>
    /// <returns>
    /// True if a collider is detected in the ground check box,
    /// otherwise false.
    /// </returns>
    public bool IsGrounded()
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

    /// <summary>
    /// Draws gizmos to visualize ground and wall check areas.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_wallCheckPosition == null || _groundCheckPosition == null)
            return;

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(
            _groundCheckPosition.position,
            new Vector2(
                _groundCheckBoxDimensions.Width,
                _groundCheckBoxDimensions.Height
            )
        );

        Gizmos.DrawWireCube(
            _wallCheckPosition.position,
            new Vector2(
                _wallCheckBoxDimensions.Width,
                _wallCheckBoxDimensions.Height
            )
        );
    }
}
