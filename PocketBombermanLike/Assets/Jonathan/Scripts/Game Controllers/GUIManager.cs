using Unity.VisualScripting;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance;
    private bool _isPauseMenuActive = false;
    public bool IsPauseMenuActive => _isPauseMenuActive;

    [SerializeField] private GUITypeCanvasPair _ingameGui;
    [SerializeField] private GUITypeCanvasPair _pauseMenuGui;
    [SerializeField] private GUITypeCanvasPair _currentMenu = new GUITypeCanvasPair(type: MenuType.None, canvas: null);

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _pauseMenuGui.Canvas.gameObject.SetActive(false);
    }

    public void SetPauseGUIActive(bool value)
    {
        Debug.Log("SetPauseGUIActive => " + value);
        _pauseMenuGui.Canvas.gameObject.SetActive(value);
        _isPauseMenuActive = value;
    }
}
