using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Countdown : MonoBehaviour
{
    [SerializeField] private TimeMS _levelTime;
    public TimeSpan TimeLeft => TimeSpan.FromSeconds(_timeLeft);
    public UnityEvent OnCountdownEnd;
    private double _timeLeft;
    private bool _hasAlreadyInvokedEvent = false;
    private bool _isActive = false;

    private void Awake()
    {
        _timeLeft = _levelTime.ToSeconds();

        if (_timeLeft <= 10)
            Debug.LogWarning("[COUNTDOWN] One of the countdowns is set to a duration shorter than 10 seconds. This might cause the game to end instantly! -");

        Debug.Log($"[COUNTDOWN] Performed initialization for object: {gameObject.name} with time: {_levelTime.ToString()} -");
    }

    public void ResetTime()
    {
        _hasAlreadyInvokedEvent = false;
        _timeLeft = _levelTime.ToSeconds();
    }

    public void StartTime()
    {
        _isActive = true;
    }

    public void PauseTime()
    {
        _isActive = false;
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