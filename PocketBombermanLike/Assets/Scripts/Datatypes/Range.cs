using System;

/// <summary>
/// Represents a numeric float range with min and max bounds.
/// Used for checks like Contains or random value generation.
/// </summary>
[Serializable]
public struct Range
{
    public float Min;
    public float Max;

    public bool Contains(float number) => number >= Min && number <= Max;

}