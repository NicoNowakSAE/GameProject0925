using System;
using UnityEngine;

/// <summary>
/// Defines the configuration data for a powerup, including its effect type, 
/// multiplier, and collision behavior.
/// </summary>
[Serializable]
public struct PowerupConfig
{
    public StatModifier.Type Type;
    public int Amount;
    public bool DestroyOnTrigger;
    public LayerMask CollisionLayer;

    public override string ToString()
    {
        return $"[Powerup: {Type} (+{Amount}) | Destroy: {DestroyOnTrigger} | Layer: {LayerMask.LayerToName(Mathf.RoundToInt(Mathf.Log(CollisionLayer.value, 2)))}]";
    }
}