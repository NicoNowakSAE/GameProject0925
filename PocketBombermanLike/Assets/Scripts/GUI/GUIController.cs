using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    private bool _isPauseMenuActive = false;
    public bool IsPauseMenuActive => _isPauseMenuActive;
    [SerializeField] private SceneController _sceneController;

    [Header("Canvas References")]
    [SerializeField] private Canvas _ingameGui;
    [SerializeField] private Canvas _pauseMenuGui;
    [SerializeField] private Canvas _mainMenuGui;
    [SerializeField] private Canvas _settingsMenuGui;

    private Canvas _currentMenuGui = null;

    private static GUIController _instance;

    public static GUIController Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void SetPauseGUIActive(bool value)
    {
        Debug.Log("[GUI MANAGER] SetPauseGUIActive => " + value);
        _pauseMenuGui.gameObject.SetActive(value);
        _isPauseMenuActive = value;
    }

    public void ResumeGame()
    {
        LevelController.Instance.SetGameState(GameState.Running);
    }

    private void ChangeMenu(Canvas targetCanvas)
    {
        Debug.Log("[GUI CONTROLLER] GUI change invoked -");

        if (_currentMenuGui != null)
        {
            Debug.Log($"[GUI CONTROLLER] Disabling current canvas -> {_currentMenuGui.gameObject.name} -");
            _currentMenuGui.gameObject.SetActive(false);
        }

        targetCanvas.gameObject.SetActive(true);
        Debug.Log($"[GUI CONTROLLER] Enabled target canvas -> {targetCanvas.gameObject.name} -");

        _currentMenuGui = targetCanvas;

        Debug.Log("[GUI CONTROLLER] GUI change completed -");
    }

    public void OpenMainMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open main menu invoked -");
        ChangeMenu(_mainMenuGui);
    }

    public void StartGameFlow()
    {
        Debug.Log("[GUI CONTROLLER] Start game flow invoked -");
        // CHANGE 01
        // _sceneController.LoadScene("HealthTest");
        _sceneController.LoadNextScene();
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open settings menu invoked -");
        ChangeMenu(_settingsMenuGui);
    }

    public void OpenLevelDoneMenu()
    {

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
