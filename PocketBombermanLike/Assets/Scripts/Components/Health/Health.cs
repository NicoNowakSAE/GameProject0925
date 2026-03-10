
using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Timer))]
public class Health : MonoBehaviour
{
    private float _currentHealth;

    /// <summary>
    /// The current health value of an entity.
    /// </summary>
    /// <returns>
    /// The current health of an entity as float.
    /// </returns>
    public float CurrentHealth => _currentHealth;

    [SerializeField] private float _baseHp;

    /// <summary>
    /// The default health value of an entity.
    /// </summary>
    /// <returns>
    /// The default health of an entity as float.
    /// </returns>
    public float BaseHealth => _baseHp;

    /// <summary>
    /// Determines whether an entity can heal above their 
    /// default health.
    /// </summary>
    [SerializeField] private bool _canHealMoreThanBase;

    /// <summary>
    /// Determines whether an entity is alive or not.
    /// </summary>
    private bool _isAlive = true;

    public bool IsAlive => _isAlive;
    /// <summary>
    /// Fires as soon as the health of the entity reaches 0.
    /// </summary>
    public UnityEvent OnEntityDeath;
    public UnityEvent OnEntityTakeDamage;

    [SerializeField] private bool _turnInactiveOnDeath = false;

    [SerializeField] private bool _useInvincibleFrames;

    [SerializeField] private float _invincibleFrameDuration;
    private Timer _invincibleTimer;
    private bool _hasAlreadyBeenAttacked = false;

    public UnityEvent OnIFrameStart;
    public UnityEvent OnIFrameEnd;

    private void Awake()
    {
        _invincibleTimer = GetComponent<Timer>();
        HealthCollection.Subscribe(gameObject, this);
        _currentHealth = _baseHp;
    }

    private void InitiateIFrameCountdown()
    {
        _invincibleTimer.StartTime();
    }

    private bool IsInvincible()
    {
        if (!_useInvincibleFrames)
            return false;

        return _invincibleTimer.TimeElapsed.TotalSeconds < _invincibleFrameDuration;
    }

    /// <summary>
    /// Removed a specified amount from an entity's health.
    /// </summary>
    /// <param name="healthRegained">
    /// Amount of health to be removed.
    /// </param>
    public void Reduce(float damageDealt)
    {
        float targetHp = _currentHealth - damageDealt;

        if (IsInvincible())
        {
            Debug.Log($"[HEALTH] Can't reduce health because target is in invincible frame state (Time left: {_invincibleFrameDuration - _invincibleTimer.TimeElapsed.TotalSeconds}) -");
            OnIFrameStart?.Invoke();
            return;
        }

        OnEntityTakeDamage.Invoke();

        if (!_hasAlreadyBeenAttacked)
        {
            Debug.Log("[HEALTH] First attack detected, setting up IFrame countdown now... -");
            _hasAlreadyBeenAttacked = true;
            OnIFrameStart?.Invoke();
            InitiateIFrameCountdown();
        }

        if (targetHp <= 0)
        {
            OnIFrameEnd.Invoke();
            Die();
            return;
        }

        _currentHealth = targetHp;
        Debug.Log($"[HEALTH] Reduced health of {gameObject.name} by {damageDealt} => Health now: {_currentHealth} -");

        _invincibleTimer.ResetTime();
    }

    public void SetAlive(bool value)
    {
        _isAlive = value;
        Debug.Log($"[HEALTH] {gameObject.name} is alive: {value} -");
    }

    /// <summary>
    /// Adds a specified amount onto an entity's health.
    /// </summary>
    /// <param name="healthRegained">
    /// Amount of health to be added.
    /// </param>
    public void Gain(float healthRegained)
    {
        float targetHp = _currentHealth + healthRegained;
        if (!_canHealMoreThanBase)
        {
            if (targetHp > _baseHp)
                targetHp = _baseHp;
        }
        _currentHealth = targetHp;
        Debug.Log($"[HEALTH] Increased health of {gameObject.name} by {healthRegained} => Health now: {_currentHealth} -");
    }

    /// <summary>
    /// Resets health of an entity to their 
    /// default health value and revives them.
    /// </summary>
    public void Reset()
    {
        _currentHealth = _baseHp;
        _hasAlreadyBeenAttacked = false;
        _invincibleTimer.ResetTime();
        _invincibleTimer.StartTime();
        SetAlive(true);
        Debug.Log($"[HEALTH] Health has been reset => Health now: {_currentHealth} -");
    }

    /// <summary>
    /// Forces instant death of an entity.
    /// Only works if the entity is not already dead.
    /// </summary>
    public void Die()
    {
        if (!_isAlive)
        {
            Debug.LogWarning("[HEALTH] You tried forcing a death on an entity which already has died -");
            return;
        }
        _currentHealth = 0;
        SetAlive(false);
        Debug.Log($"[HEALTH] {gameObject.name} died -");

        if (_turnInactiveOnDeath)
            gameObject.SetActive(false);

        OnEntityDeath?.Invoke();
    }

    private void Update()
    {
        if (!_useInvincibleFrames)
            return;

        if (_invincibleTimer.TimeElapsed.TotalSeconds >= _invincibleFrameDuration)
            OnIFrameEnd?.Invoke();
    }
}
