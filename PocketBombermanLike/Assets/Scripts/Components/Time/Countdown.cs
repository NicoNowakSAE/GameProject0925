using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TimeMS _time;
    
    /// <summary>
    /// Gets the remaining duration of the countdown as a <see cref="TimeSpan"/>.
    /// </summary>
    public TimeSpan TimeLeft => TimeSpan.FromSeconds(_timeLeft);
    
    public UnityEvent OnCountdownEnd;
    private double _timeLeft;
    private bool _hasAlreadyInvokedEvent = false;
    private bool _isActive = false;

    private void Awake()
    {
        _timeLeft = _time.ToSeconds();

        if (_timeLeft <= 10)
            Debug.LogWarning("[COUNTDOWN] One of the countdowns is set to a duration shorter than 10 seconds. -");

        Debug.Log($"[COUNTDOWN] Performed initialization for object: {gameObject.name} with time: {_time.ToString()} -");
    }

    /// <summary>
    /// Resets the countdown state, allowing <see cref="OnCountdownEnd"/> to be triggered again 
    /// and restoring <see cref="_timeLeft"/> from the current <see cref="_time"/> setting.
    /// </summary>
    public void ResetTime()
    {
        _hasAlreadyInvokedEvent = false;
        _timeLeft = _time.ToSeconds();
    }

    /// <summary>
    /// Enables the countdown progression in the <see cref="Update"/> loop.
    /// </summary>
    public void StartTime()
    {
        _isActive = true;
    }

    /// <summary>
    /// Pauses the countdown progression without resetting the current <see cref="_timeLeft"/>.
    /// </summary>
    public void PauseTime()
    {
        _isActive = false;
    }

    /// <summary>
    /// Updates the target duration of the countdown. 
    /// Note: Does not automatically apply to the current <see cref="_timeLeft"/> until <see cref="ResetTime"/> is called.
    /// </summary>
    /// <param name="time">The new time configuration to use.</param>
    public void SetTime(TimeMS time)
    {
        _time = time;
    }

    private void Update()
    {
        if (_isActive)
        {
            if (_timeLeft <= 0 && !_hasAlreadyInvokedEvent)
            {
                OnCountdownEnd?.Invoke();
                _hasAlreadyInvokedEvent = true;
                return;
            }

            _timeLeft -= Time.deltaTime;
        } 
    }
}