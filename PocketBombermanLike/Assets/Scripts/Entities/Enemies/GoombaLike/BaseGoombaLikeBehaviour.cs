using UnityEngine;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// Provides the base behaviour for Goomba-like enemies.
/// Handles constant movement and basic direction flipping
/// when encountering walls or edges.
/// </summary>
public class BaseGoombaLikeBehaviour : EntityBaseBehaviour, IBombHit
{
    #region Components / References

    [Header("Components / References")]
    /// <summary>
    /// Provides environmental checks such as ground and wall detection.
    /// </summary>
    private Sensor _sensor;

    /// <summary>
    /// Controls movement speed and facing direction.
    /// </summary>
    private MovementController _movementController;

    private Health _health;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        _sensor = GetComponent<Sensor>();
        _health = GetComponent<Health>();
        _health.OnEntityDeath.AddListener(OnDeath);
    }

    #endregion

    #region Core Behaviour Methods

    /// <summary>
    /// Handles physics-based movement.
    /// Moves constantly at base speed.
    /// </summary>
    public override void FixedTick()
    {
        _movementController.Move(_movementController.BaseSpeed);
    }

    /// <summary>
    /// Handles non-physics logic.
    /// Flips direction when hitting a wall or reaching an edge.
    /// </summary>
    public override void Tick()
    {
        if (!_sensor.IsGrounded() || _sensor.IsWallAhead())
        {
            _movementController.Flip();
        }
    }

    public void Hit(int dmg) => _health.Reduce(dmg);

    public override void OnDeath()
    {
        EnemyCollection.Unsubscribe(gameObject);
    }

    #endregion
}
