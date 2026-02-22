using System;

/// <summary>
/// A serializable data structure that links a specific enemy state to a movement speed.
/// Used to define different movement behaviors in the inspector for states like Patrolling or Chasing.
/// </summary>
[Serializable]
public struct StateMovementspeedPair
{
    /// <summary>
    /// The specific state this speed applies to.
    /// </summary>
    public ChasingEnemyState State;

    /// <summary>
    /// The movement speed value associated with this state.
    /// </summary>
    public float Speed;
}