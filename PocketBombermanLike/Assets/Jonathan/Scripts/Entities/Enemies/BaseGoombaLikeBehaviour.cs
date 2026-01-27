using UnityEngine;

/// <summary>
/// Provides the base behaviour for Goomba-like enemies.
/// Handles constant movement and basic direction flipping
/// when encountering walls or edges.
/// </summary>
public class BaseGoombaLikeBehaviour : MonoBehaviour, IGoombaLikeBehaviour
{
    /// <summary>
    /// Provides environmental checks such as ground and wall detection.
    /// </summary>
    private Sensor _sensor;

    /// <summary>
    /// Controls movement speed and facing direction.
    /// </summary>
    private MovementController _movementController;

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        _sensor = GetComponent<Sensor>();
    }

    /// <summary>
    /// Handles physics-based movement.
    /// Moves constantly at base speed.
    /// </summary>
    public void FixedTick()
    {
        _movementController.Move(_movementController.BaseSpeed);
    }

    /// <summary>
    /// Handles non-physics logic.
    /// Flips direction when hitting a wall or reaching an edge.
    /// </summary>
    public void Tick()
    {
        if (!_sensor.IsGrounded() || _sensor.IsWallAhead())
        {
            _movementController.Flip();
        }
    }
}
