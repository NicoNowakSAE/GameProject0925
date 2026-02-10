using System;
using UnityEngine;

[Serializable]
public struct GUITypeCanvasPair
{
    public MenuType Type;
    public Canvas Canvas;

    public GUITypeCanvasPair(MenuType type, Canvas canvas)
    {
        Type = type;
        Canvas = canvas;
    }

    public override string ToString() => $"Type: {Type} | Canvas: {Canvas}";
}