using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public List<GUITypeCanvasPair> MenuList = new List<GUITypeCanvasPair>();
    private GUITypeCanvasPair _currentMenuScope;
    [SerializeField] private Canvas _startMenu;
    [SerializeField] private SceneController _sceneController;

    private void Start()
    {
        Debug.Log("[GUI CONTROLLER] Start initializing GUI controller -");

        _currentMenuScope = new GUITypeCanvasPair(MenuType.Main, _startMenu);

        if (_startMenu == null)
        {
            Debug.Log("[GUI CONTROLLER] Start menu is NULL -");
            return;
        }

        int disabledCount = 0;

        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.GetComponent<GUIAsset>() != null))
        {
            Canvas canvas = obj.GetComponent<Canvas>();

            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
                disabledCount++;
                Debug.Log($"[GUI CONTROLLER] Disabled canvas -> {canvas.gameObject.name} -");
            }
        }

        Debug.Log($"[GUI CONTROLLER] Total canvases disabled: {disabledCount} -");

        _currentMenuScope.Canvas.gameObject.SetActive(true);
        Debug.Log($"[GUI CONTROLLER] Activated start menu -> {_startMenu.name} -");
    }

    private GUITypeCanvasPair GetTypeCanvasPair(MenuType type)
    {
        Debug.Log($"[GUI CONTROLLER] Searching for menu type -> {type} -");

        foreach (GUITypeCanvasPair typeCanvasPair in MenuList)
        {   
            if (typeCanvasPair.Type == type)
            {
                Debug.Log($"[GUI CONTROLLER] Found canvas -> {typeCanvasPair.Canvas?.name} -");
                return typeCanvasPair;
            }
        }

        Debug.Log($"[GUI CONTROLLER] Menu type not found -> {type} -");
        return new GUITypeCanvasPair();
    }

    private void ChangeMenu(MenuType targetMenuType)
    {
        Debug.Log("[GUI CONTROLLER] GUI change invoked -");

        GUITypeCanvasPair typeCanvasPair = GetTypeCanvasPair(targetMenuType);

        if (typeCanvasPair.Canvas == null)
        {
            Debug.Log($"[GUI CONTROLLER] Target canvas is NULL -> {targetMenuType} -");
            return;
        }

        if (_currentMenuScope.Canvas != null)
        {
            Debug.Log($"[GUI CONTROLLER] Disabling current canvas -> {_currentMenuScope.Canvas.name} -");
            _currentMenuScope.Canvas.gameObject.SetActive(false);
        }

        typeCanvasPair.Canvas.gameObject.SetActive(true);
        Debug.Log($"[GUI CONTROLLER] Enabled target canvas -> {typeCanvasPair.Canvas.name} -");

        _currentMenuScope = typeCanvasPair;

        Debug.Log("[GUI CONTROLLER] GUI change completed -");
    }

    public void OpenMainMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open main menu invoked -");
        ChangeMenu(MenuType.Main);
    }

    public void StartGameFlow()
    {
        Debug.Log("[GUI CONTROLLER] Start game flow invoked -");
        _sceneController.LoadScene("HealthTest");
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open settings menu invoked -");
        ChangeMenu(MenuType.Settings);
    }

    public void OpenQuitMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open quit menu invoked -");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}
