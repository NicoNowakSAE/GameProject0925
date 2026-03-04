using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    private bool _isPauseMenuActive = false;
    public bool IsPauseMenuActive => _isPauseMenuActive;
    [SerializeField] private SceneController _sceneController;

    [Header("GUI Canvas References")]
    [Header("Ingame GUI")]
    [SerializeField] private Canvas _ingameGui;

    [Header("Pause GUI")]
    [SerializeField] private Canvas _pauseMenuGui;

    [Header("Main Menu GUI")]
    [SerializeField] private Canvas _mainMenuGui;

    [Header("Settings Menu GUI")]
    [SerializeField] private Canvas _settingsMenuGui;

    [Header("Level Done Menu GUI")]
    [SerializeField] private Canvas _levelDoneMenuGui;

    [Header("Level Win Menu GUI")]
    [SerializeField] private Canvas _levelWinMenuGui;

    [Header("Level Lose Menu GUI")]
    [SerializeField] private Canvas _levelLoseMenuGui;

    private Canvas _currentMenuGui = null;

    private static GUIController _instance;

    public static GUIController Instance => _instance;

    private void Awake()
    {
        _instance = this;

        LevelController.Instance.OnGameLost.AddListener(OpenLoseMenu);
    }

    public void SetPauseGUIActive(bool value)
    {
        Debug.Log("[GUI MANAGER] SetPauseGUIActive => " + value);
        _pauseMenuGui.gameObject.SetActive(value);
        _isPauseMenuActive = value;
    }

    public void ButtonClick_ContinueGame()
    {
        SaveLoadManager.Instance.Load();
        LevelController.Instance.SetGameState(GameState.Running);
    }

    public void ButtonClick_ResumeGame()
    {
        LevelController.Instance.SetGameState(GameState.Running);
    }

    public void ButtonClick_BackToMainMenu()
    {
        SceneController.Instance.LoadScene("MainMenu");
        LevelController.Instance.SetGameState(GameState.None);
    }

    public void ButtonClick_LoadNextLevel()
    {
        SceneController.Instance.LoadNextScene();
        LevelController.Instance.SetGameState(GameState.Running);
    }

    public void ButtonClick_RetryLevel()
    {
        SceneController.Instance.LoadScene(SceneController.Instance.CurrentScene.buildIndex);
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
        SceneController.Instance.LoadNextScene();
    }

    public void OpenWinMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open win menu invoked -");
        LevelController.Instance.SetGameState(GameState.InBetween);
        ChangeMenu(_levelWinMenuGui);
    }
    
    public void OpenLoseMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open lose menu invoked -");
        LevelController.Instance.SetGameState(GameState.InBetween);
        ChangeMenu(_levelLoseMenuGui);
    }
    public void OpenSettingsMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open settings menu invoked -");
        ChangeMenu(_settingsMenuGui);
    }

    public void OpenLevelDoneMenu()
    {
        Debug.Log("[GUI CONTROLLER] Open level done menu invoked -");
        ChangeMenu(_levelDoneMenuGui);
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
