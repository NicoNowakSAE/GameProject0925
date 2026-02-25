using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Timer))]
public class LevelController : MonoBehaviour
{
    private int _enemyLayerMask;
    private int _playerLayerMask;

    [Tooltip("Determines the distance between the player and the end anchor at which the level is considered completed.")]
    [Range(0.5f, 5.0f)]
    [SerializeField] private float _levelEndDistanceTreshold = 1.5f;

    private GameObject _startAnchor;
    public Vector3 LevelStartPosition => _startAnchor.transform.position;
    private GameObject _endAnchor;
    public Vector3 LevelEndPosition => _endAnchor.transform.position;
    private GameObject _player;
    private Health _playerHealth;
    public UnityEvent OnPlayerTouchEnd;
    private bool _isPlayerEndTouchSatisfied = false;
    private Timer _timer;
    public TimeSpan TimeElapsed => _timer.TimeElapsed;
    private int _enemiesRemaining = 999;
    public int EnemiesRemaining => _enemiesRemaining;
    private int _heartsCount = 3;
    public int HeartsCount => _heartsCount;
    private string[] _activePowerups;
    public string[] ActivePowerups => _activePowerups;
    [SerializeField] private int _currentLevel = 999;
    public int CurrentLevel => _currentLevel;
    public float PlayerHealth => _playerHealth.CurrentHealth;

    private GameState _currentGameState = GameState.Running;
    private PlayerInput _playerInput;
    private GUIController _guiController;
    public static LevelController Instance;

    private GameObject[] GetAllObjectsInScene() => FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);

    private void Awake()
    {
        _playerLayerMask = LayerMask.NameToLayer("Player");
        _enemyLayerMask = LayerMask.NameToLayer("Enemy");
        _playerInput = FindFirstObjectByType<PlayerInput>();
        _guiController = FindFirstObjectByType<GUIController>();

        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private int GetEnemyCount()
    {
        GameObject[] enemiesInScene = GetAllObjectsInScene().Where(e => e.layer == _enemyLayerMask).ToArray();
        return enemiesInScene.Count();
    }

    private void SpawnPlayer()
    {
        _player.transform.position = _startAnchor.transform.position;
        _playerHealth.Gain(_playerHealth.BaseHealth);
        _playerHealth.SetAlive(true);
    }

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

    private void Start()
    {
        _timer = GetComponent<Timer>();
        _timer.StartTime();

        FetchLevelAnchors();
        FetchPlayer();

        _playerHealth.OnEntityDeath.AddListener(SpawnPlayer); // ?
        OnPlayerTouchEnd.AddListener(LevelDone); // CHANGE 06
        _enemiesRemaining = GetEnemyCount();

        Debug.Log($"[LEVEL CONTROLLER] Found {_enemiesRemaining} enemies on Start() -");
        Debug.Log($"[LEVEL CONTROLLER] Found end anchor {_endAnchor != null} ({_endAnchor.transform.position.ToString()} -");
        Debug.Log($"[LEVEL CONTROLLER] Found start anchor {_startAnchor != null} ({_startAnchor.transform.position.ToString()} -");
        Debug.Log($"[LEVEL CONTROLLER] Found player: {_player != null}");
    }

    public void RemoveEntity()
    {
        // todo: refactor this
        _enemiesRemaining--;

        if (_enemiesRemaining < 0)
            _enemiesRemaining = 0;
    }

    // CHANGE 07
    public void LevelDone()
    {
        Debug.Log("LEVEL DONE!!");

        SceneController.Instance.LoadNextScene();
        SetGameState(GameState.InBetween);
        return;
        
    }

    /// <summary>
    /// Switches between <see cref="GameState.Paused"/> and <see cref="GameState.Running"/>.
    /// </summary>
    public void ToggleState()
    {
        print("[LEVEL CONTROLLER] ToggleState() Invoked... -");

        switch (_currentGameState)
        {
            case GameState.Paused:
                SetGameState(GameState.Running);
                break;
            case GameState.Running:
                SetGameState(GameState.Paused);
                break;
        }
    }

    public void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Paused:
                _guiController.SetPauseGUIActive(true);
                Time.timeScale = 0;
                break;
            case GameState.Running:
                Time.timeScale = 1;
                _guiController.SetPauseGUIActive(false);
                break;
            // CHANGE 04
            case GameState.InBetween:
                Time.timeScale = 0;
                _guiController.OpenLevelDoneMenu();
                break;
        }

        print($"[LEVEL CONTROLLER] Setting new game state: {state} -");

        _currentGameState = state;
    }


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
                OnPlayerTouchEnd?.Invoke();
            }
        }
    }
}
