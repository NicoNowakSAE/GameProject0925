using System;
using UnityEngine;

/// <summary>
/// A serializable container that maps a specific <see cref="Material"/> to a <see cref="StatModifier.Type"/>.
/// Used by systems like <see cref="PowerupCore"/> to visually differentiate powerup types.
/// </summary>
[Serializable]
public struct StatModifierMaterialPair
{
    public Material Material;
    public StatModifier.Type StatType;
}