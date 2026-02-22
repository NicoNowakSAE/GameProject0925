using System;
using UnityEngine;

/// <summary>
/// A simple serializable data structure representing a 2D rectangular region.
/// </summary>
[Serializable]
public struct Area
{
    /// <summary>
    /// The vertical dimension of the area.
    /// </summary>
    public float Height;

    /// <summary>
    /// The horizontal dimension of the area.
    /// </summary>
    public float Width;
}