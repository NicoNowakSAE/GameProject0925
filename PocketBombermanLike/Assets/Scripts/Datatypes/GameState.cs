using System;

[Serializable]
public enum GameState
{
    Paused,
    Running,
    Loading,
    InBetween, // CHANGE 03
    None
}