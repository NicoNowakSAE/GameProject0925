using System;
using UnityEngine;

public class GUIAsset : MonoBehaviour
{
    [SerializeField] private MenuType _menuType;
    public MenuType MenuType => _menuType;

    private GUIController _guiController;

    private void Awake()
    {
        _guiController = FindFirstObjectByType<GUIController>();

        Canvas canvas = GetComponent<Canvas>();
        GUITypeCanvasPair newTypeCanvasPair = new GUITypeCanvasPair(_menuType, canvas);
        _guiController.MenuList.Add(newTypeCanvasPair);

        Debug.Log($"[GUI ASSET] Adding object to list: {newTypeCanvasPair.Canvas.gameObject.name} -");
    }
}
