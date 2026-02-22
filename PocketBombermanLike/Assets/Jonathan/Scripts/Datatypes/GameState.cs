using System;

/// <summary>
/// Defines the high-level states of the game flow.
/// </summary>
[Serializable]
public enum GameState
{
    /// <summary>
    /// The game is currently paused. Logic updates should typically be suspended.
    /// </summary>
    Paused,

    /// <summary>
    /// The game is actively running. Standard gameplay logic and physics are processed.
    /// </summary>
    Running,

    /// <summary>
    /// The game is currently in a transition or loading screen.
    /// </summary>
    Loading,

    /// <summary>
    /// A default or uninitialized state. Used to indicate that no specific state is currently active.
    /// </summary>
    None
}