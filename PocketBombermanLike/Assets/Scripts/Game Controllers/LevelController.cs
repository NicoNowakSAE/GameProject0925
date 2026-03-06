using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Countdown))]
public class LevelController : MonoBehaviour
{
    private int _enemyLayerMask;
    private int _playerLayerMask;

    [Tooltip("Determines the distance between the player and the end anchor at which the level is considered completed.")]
    [Range(0.5f, 5.0f)]
    [SerializeField] private float _levelEndDistanceTreshold = 1.5f;

    public UnityEvent OnGameLost;
    private GameObject _startAnchor;
    public Vector3 LevelStartPosition => _startAnchor.transform.position;
    private GameObject _endAnchor;
    public Vector3 LevelEndPosition => _endAnchor.transform.position;
    private GameObject _player;
    private Health _playerHealth;
    public UnityEvent OnPlayerTouchEnd;
    private bool _isPlayerEndTouchSatisfied = false;
    private Countdown _countdown;
    public TimeSpan LevelTimeLeft => _countdown.TimeLeft;
    public int EnemiesRemaining => EnemyCollection.EnemyList.Count;
    private int _heartsCount = 3;
    public int HeartsCount => _heartsCount;

    public Health PlayerHealth => _playerHealth;

    private GameState _currentGameState = GameState.Running;
    private PlayerInput _playerInput;
    public static LevelController Instance;

    private GameObject[] GetAllObjectsInScene() => FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        _playerLayerMask = LayerMask.NameToLayer("Player");
        _enemyLayerMask = LayerMask.NameToLayer("Enemy");
        _playerInput = FindFirstObjectByType<PlayerInput>();
        _countdown = GetComponent<Countdown>();

        SceneController.Instance.OnSceneLoadFinished.AddListener(() => SetGameState(GameState.Running));
        FetchPlayer();

    }

    private void RemovePlayerHeart()
    {
        if (_heartsCount <= 0)
        {
            Debug.Log("[LEVEL CONTROLLER] Game lost! -");
            OnGameLost?.Invoke();
            return;
        }

        _heartsCount--;
        Debug.Log($"[LEVEL CONTROLLER] Remvoed 1 player heart. Hearts remaining: {_heartsCount} -");
    }

    public void LevelLostFlow()
    {
        Debug.Log("[LEVEL CONTROLLER] Running level lost flow -");
    }

    private void SpawnPlayer()
    {
        Debug.Log("[LEVEL CONTROLLER] Respawning player... -");
        _player.transform.position = _startAnchor.transform.position;
        _playerHealth.SetAlive(true);
        _playerHealth.Reset();
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
        GameObject[] playerObj = GetAllObjectsInScene().Where(obj => obj.layer == _playerLayerMask).ToArray();

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
        _countdown.StartTime();

        FetchLevelAnchors();

        _playerHealth.OnEntityDeath.AddListener(SpawnPlayer);
        _playerHealth.OnEntityDeath.AddListener(RemovePlayerHeart);
        _countdown.OnCountdownEnd.AddListener(LevelLostFlow);

        EnemyCollection.Cleanup();
        HealthCollection.Cleanup();
        
        OnPlayerTouchEnd.AddListener(LevelDone); // CHANGE 06

        Debug.Log($"[LEVEL CONTROLLER] Found {EnemiesRemaining} enemies on Start() -");
        Debug.Log($"[LEVEL CONTROLLER] Found end anchor {_endAnchor != null} ({_endAnchor.transform.position.ToString()} -");
        Debug.Log($"[LEVEL CONTROLLER] Found start anchor {_startAnchor != null} ({_startAnchor.transform.position.ToString()} -");
        Debug.Log($"[LEVEL CONTROLLER] Found player: {_player != null}");

        if (_startAnchor != null && _player != null)
        {
            Debug.Log($"[LEVEL CONTROLLER] Attempting to spawn player... ");
            SpawnPlayer();
        }


    }

    // CHANGE 07
    public void LevelDone()
    {
        print("[LEVEL CONTROLLER] Running level done flow... -");

        SaveLoadManager.Instance?.Save();
        GUIController.Instance?.OpenLevelDoneMenu();
        SetGameState(GameState.InBetween);
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

    // public void SetLevelIndex(int idx)
    // {
    //     SceneController._currentLevel = idx;
    //     Debug.Log($"[LEVEL CONTROLLER] Setting level index to: {idx} -");
    // }

    public void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Paused:
                GUIController.Instance?.SetPauseGUIActive(true);
                Time.timeScale = 0;
                break;
            case GameState.Running:
                Time.timeScale = 1;
                GUIController.Instance?.SetPauseGUIActive(false);
                break;
            case GameState.InBetween:
                Time.timeScale = 0;
                break;
            case GameState.None:
                Time.timeScale = 1;
                break;
        }

        print($"[LEVEL CONTROLLER] Setting new game state: {state} -");

        _currentGameState = state;
    }


    private void Update()
    {
        if (_endAnchor != null)
        {
            if (EnemiesRemaining <= 0 && !_endAnchor.activeInHierarchy)
            {
                _endAnchor.SetActive(true);
                Debug.Log("[LEVEL CONTROLLER] All enemies killed; End anchor is now active -");
            }

        }

        if (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, _endAnchor.transform.position) < _levelEndDistanceTreshold && !_isPlayerEndTouchSatisfied && EnemiesRemaining <= 0)
            {
                Debug.Log("[LEVEL CONTROLLER] Level end condition has been satisfied => Invoking OnPlayerTouchEnd Event now -");
                _isPlayerEndTouchSatisfied = true;
                OnPlayerTouchEnd?.Invoke();
            }
        }

        if (_countdown.TimeLeft.TotalSeconds <= 0 && _currentGameState == GameState.Running)
        {
            OnGameLost?.Invoke();
        }
    }

    
}
