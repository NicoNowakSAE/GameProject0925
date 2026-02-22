using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the core level lifecycle, including player spawning, enemy tracking, 
/// pause states, and level completion conditions.
/// </summary>
[RequireComponent(typeof(Timer))]
public class LevelController : MonoBehaviour
{
    private int _enemyLayerMask;
    private int _playerLayerMask;

    [Tooltip("Determines the distance between the player and the end anchor at which the level is considered completed.")]
    [Range(0.5f, 5.0f)]
    [SerializeField] private float _levelEndDistanceTreshold = 1.5f;

    private GameObject _startAnchor;
    /// <summary> The world position of the level's start anchor. </summary>
    public Vector3 LevelStartPosition => _startAnchor.transform.position;
    
    private GameObject _endAnchor;
    /// <summary> The world position of the level's end anchor. </summary>
    public Vector3 LevelEndPosition => _endAnchor.transform.position;
    
    private GameObject _player;
    private Health _playerHealth;
    
    /// <summary> Event triggered when the player reaches the end anchor after all enemies are defeated. </summary>
    public UnityEvent OnPlayerTouchEnd;
    
    private bool _isPlayerEndTouchSatisfied = false;
    private Timer _timer;
    
    /// <summary> The total time elapsed since the level started. </summary>
    public TimeSpan TimeElapsed => _timer.TimeElapsed;
    
    private int _enemiesRemaining = 999;
    /// <summary> Current count of active enemies in the level. </summary>
    public int EnemiesRemaining => _enemiesRemaining;
    
    private int _heartsCount = 3;
    /// <summary> Current count of player hearts/lives. </summary>
    public int HeartsCount => _heartsCount;
    
    private string[] _activePowerups;
    /// <summary> List of powerup identifiers currently active on the player. </summary>
    public string[] ActivePowerups => _activePowerups;
    
    [SerializeField] private int _currentLevel = 999;
    /// <summary> The index or ID of the current level. </summary>
    public int CurrentLevel => _currentLevel;
    
    /// <summary> The current health value of the player entity. </summary>
    public float PlayerHealth => _playerHealth.CurrentHealth;
    
    private GameState _currentGameState = GameState.Running;
    private PlayerInput _playerInput;
    private GUIManager _guiManager;
    
    /// <summary> Static reference to the LevelController for global access. </summary>
    public static LevelController Instance;

    /// <summary>
    /// Helper method to retrieve all GameObjects currently active in the scene.
    /// </summary>
    private GameObject[] GetAllObjectsInScene() => FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);

    private void Awake()
    {
        _playerLayerMask = LayerMask.NameToLayer("Player");
        _enemyLayerMask = LayerMask.NameToLayer("Enemy");
        _playerInput = FindFirstObjectByType<PlayerInput>();
        _guiManager = FindFirstObjectByType<GUIManager>();

        Instance = this;
    }

    /// <summary>
    /// Scans the scene to count GameObjects associated with the enemy layer.
    /// </summary>
    private int GetEnemyCount()
    {
        GameObject[] enemiesInScene = GetAllObjectsInScene().Where(e => e.layer == _enemyLayerMask).ToArray();
        return enemiesInScene.Count();
    }

    /// <summary>
    /// Resets the player's position to the start anchor and restores their health.
    /// </summary>
    private void SpawnPlayer()
    {
        _player.transform.position = _startAnchor.transform.position;
        _playerHealth.Gain(_playerHealth.BaseHealth);
        _playerHealth.SetAlive(true);
    }
    
    /// <summary>
    /// Locates and assigns the Start and End LevelAnchor objects in the scene.
    /// </summary>
    private void FetchLevelAnchors()
    {
        foreach (GameObject obj in GetAllObjectsInScene())
        {
            if (obj.TryGetComponent(out LevelAnchor anchor))
            {
                switch (anchor.AnchorType)
                {
                    case LevelAnchorType.Start:
                        if (_startAnchor != null)
                            Debug.LogWarning("[LEVEL CONTROLLER] Position of start anchor has been overwritten -");

                        _startAnchor = obj;
                        break;

                    case LevelAnchorType.End:
                        if (_endAnchor != null)
                            Debug.LogWarning("[LEVEL CONTROLLER] Position of end anchor has been overwritten -");

                        _endAnchor = obj;
                        break;
                }
            }
        }

        if (_endAnchor != null)
            _endAnchor.SetActive(false);
        else
            Debug.LogWarning("[LEVEL CONTROLLER] No end anchor found -");

        if (_startAnchor == null)
            Debug.LogWarning("[LEVEL CONTROLLER] No start anchor found -");
    }

    /// <summary>
    /// Finds the player object in the scene based on layer and hierarchy status.
    /// </summary>
    private void FetchPlayer()
    {
        GameObject[] playerObj = GetAllObjectsInScene().Where(obj => obj.layer == _playerLayerMask).Where(obj => obj.transform.parent == null).ToArray();

        if (playerObj.Count() == 0)
        {
            Debug.LogWarning("[LEVEL CONTROLLER] Player not found -");
            return;
        }

        if (playerObj.Count() > 1)
        {
            Debug.LogWarning("[LEVEL CONTROLLER] Multiple players found. This will break the game -");
            return;
        }

        _player = playerObj.First();
        _playerHealth = _player.GetComponent<Health>();
    }

    /// <summary>
    /// Starts the timer, fetches scene references, and initializes enemy tracking.
    /// </summary>
    private void Start()
    {
        _timer = GetComponent<Timer>();
        _timer.StartTime();

        FetchLevelAnchors();
        FetchPlayer();

        _playerHealth.OnEntityDeath.AddListener(SpawnPlayer);
        _enemiesRemaining = GetEnemyCount();

        Debug.Log($"[LEVEL CONTROLLER] Found {_enemiesRemaining} enemies on Start() -");
        Debug.Log($"[LEVEL CONTROLLER] Found end anchor {_endAnchor != null} ({_endAnchor.transform.position.ToString()} -");
        Debug.Log($"[LEVEL CONTROLLER] Found start anchor {_startAnchor != null} ({_startAnchor.transform.position.ToString()} -");
        Debug.Log($"[LEVEL CONTROLLER] Found player: {_player != null}");
    }

    /// <summary>
    /// Decrements the enemy counter. Called when an enemy is defeated.
    /// </summary>
    public void RemoveEntity()
    {
        // todo: refactor this
        _enemiesRemaining--;

        if (_enemiesRemaining < 0)
            _enemiesRemaining = 0;
    }

    /// <summary>
    /// Sets the game state to Paused and stops the time scale.
    /// </summary>
    public void PauseGame()
    {
        if (_currentGameState == GameState.Paused)
            return;
        
        _currentGameState = GameState.Paused;
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Sets the game state to Running and restores the time scale.
    /// </summary>
    public void ResumeGame()
    {
        if (_currentGameState == GameState.Running)
            return;

        _currentGameState = GameState.Running;
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Sets the game state and updates the GUI Manager and time scale accordingly.
    /// </summary>
    /// <param name="state">The target GameState to transition to.</param>
    public void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Paused:
            _guiManager.SetPauseGUIActive(true);
                Time.timeScale = 0;
                break;
            case GameState.Running:
                Time.timeScale = 1;
                _guiManager.SetPauseGUIActive(false);
                break;
        }
    }

    /// <summary>
    /// Monitors win conditions (enemies cleared + distance to end) and handles pause input toggling.
    /// </summary>
    private void Update()
    {
        if (_enemiesRemaining <= 0 && !_endAnchor.activeInHierarchy)
        {
            _endAnchor.SetActive(true);
            Debug.Log("[LEVEL CONTROLLER] All enemies killed; End anchor is now active -");
        }

        if (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, _endAnchor.transform.position) < _levelEndDistanceTreshold && !_isPlayerEndTouchSatisfied && _enemiesRemaining <= 0)
            {
                Debug.Log("[LEVEL CONTROLLER] Level end condition has been satisfied => Invoking OnPlayerTouchEnd Event now -");
                _isPlayerEndTouchSatisfied = true;
                OnPlayerTouchEnd.Invoke();
            }
        }

        if (_playerInput.TogglePause.WasPressedThisFrame())
        {
            switch (_currentGameState)
            {
                case GameState.Paused:
                    _currentGameState = GameState.Running;
                    Time.timeScale = 1.0f;
                    break;
                case GameState.Running:
                    _currentGameState = GameState.Paused;
                    Time.timeScale = 0.0f;
                    break;
            }
        }
    }
}