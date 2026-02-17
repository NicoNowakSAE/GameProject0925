using Unity.VisualScripting;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private bool _isPauseMenuActive = false;
    public bool IsPauseMenuActive => _isPauseMenuActive;

    [SerializeField] private GUITypeCanvasPair _ingameGui;
    [SerializeField] private GUITypeCanvasPair _pauseMenuGui;
    [SerializeField] private GUITypeCanvasPair _currentMenu = new GUITypeCanvasPair(type: MenuType.None, canvas: null);

    public void SetPauseGUIActive(bool value)
    {
        Debug.Log("[GUI MANAGER] SetPauseGUIActive => " + value);
        _pauseMenuGui.Canvas.gameObject.SetActive(value);
        _isPauseMenuActive = value;
    }
}
