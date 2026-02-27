using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Timer : MonoBehaviour
{
    public TimeSpan TimeElapsed => TimeSpan.FromSeconds(_timeElapsed);
    private double _timeElapsed = 0.0f;
    private bool _isActive = false;

    public void ResetTime()
    {
        _timeElapsed = 0.0f;
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
            _timeElapsed += Time.deltaTime;
    }
}