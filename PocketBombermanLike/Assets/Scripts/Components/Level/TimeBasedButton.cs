using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TimeBasedButton : MonoBehaviour, IBombHit
{
    [SerializeField] private Triggerable _triggerable;
    [SerializeField] private TextMeshPro _timeLabel;
    [SerializeField] private bool _isObjectActiveByDefault = false;
    [SerializeField] private bool _useTime = true;
    [SerializeField] private TimeMS _time = new() { Minutes = 0, Seconds = 10 };
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private MeshRenderer _meshRenderer;
    private bool _isTriggerableActive;
    private bool _isCycleActive = false;
    private Material _material;
    private bool _hasAlreadyBeenUsed = false;

    private void Awake()
    {
        _timeLabel.gameObject.SetActive(false);

        _material = Instantiate(_flashMaterial);
        _material.SetFloat("_Fraction", 100);

        List<Material> materials = _meshRenderer.materials.ToList();

        materials.Add(_material);
        _meshRenderer.materials = materials.ToArray();
    }

    private void Start()
    {
        _isTriggerableActive = _isObjectActiveByDefault;
        SetTriggerableActive(_isTriggerableActive);
        Debug.Log($"[TIME BASED BUTTON] Setting {_triggerable.gameObject.name} to {_isObjectActiveByDefault} for default -");
    }

    /// <summary>
    /// Manages the timed lifecycle of the trigger. Updates the <see cref="_flashMaterial"/> 
    /// progress and calls <see cref="SwitchTriggerable"/> once the duration is reached.
    /// </summary>
    private IEnumerator ObjectTriggerCycle()
    {
        Debug.Log($"[TIME BASED BUTTON] Started object trigger cycle. Waiting for time: {_time.ToSeconds()} seconds -");

        _isCycleActive = true;
        float timeElapsed = 0f;
        float fraction;

        _timeLabel.SetText(_time.ToSeconds().ToString());
        _timeLabel.gameObject.SetActive(true);

        while (_isCycleActive)
        {
            _timeLabel.SetText(((int)_time.ToSeconds() - (int)timeElapsed).ToString());

            if (timeElapsed >= _time.ToSeconds())
                _isCycleActive = false;

            fraction = 100 - (float)(timeElapsed / _time.ToSeconds()) * 100;

            _material.SetFloat("_Fraction", fraction);

            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        SwitchTriggerable();

        _material.SetFloat("_Fraction", 100);
        _timeLabel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the active state of <see cref="_triggerable"/> and updates the 
    /// shader properties of <see cref="_flashMaterial"/> accordingly.
    /// </summary>
    /// <param name="value">The target activation state.</param>
    private void SetTriggerableActive(bool value)
    {
        if (value == true)
        {
            _triggerable.Activate();
            _material.SetFloat("_Fraction", 100);
        }
        else
        {
            _triggerable.Deactivate();
            _material.SetFloat("_Fraction", 0);
        }

        Debug.Log($"[TIME BASED BUTTON] Switched {_triggerable.gameObject.name} to be active: {value} -");
    }

    /// <summary>
    /// Toggles the current state of <see cref="_triggerable"/> by invoking <see cref="SetTriggerableActive"/>.
    /// </summary>
    private void SwitchTriggerable()
    {
        switch (_triggerable.IsActive)
        {
            case true:
                SetTriggerableActive(false);
                if (_hasAlreadyBeenUsed)
                    _material.SetFloat("_Fraction", 0);
                break;

            case false:
                SetTriggerableActive(true);
                if (_hasAlreadyBeenUsed)
                    _material.SetFloat("_Fraction", 100);
                break;
        }
    }

    /// <summary>
    /// Implementation of <see cref="IBombHit"/>. Triggers the switching logic and 
    /// initiates the <see cref="ObjectTriggerCycle"/> if timer usage is enabled.
    /// </summary>
    /// <param name="dmg">Incoming damage value (currently used for logging only).</param>
    public void Hit(int dmg)
    {
        _hasAlreadyBeenUsed = true;

        Debug.Log($"[TIME BASED BUTTON] Hit detected. Performing trigger logic now -");

        if (_isCycleActive)
            return;

        SwitchTriggerable();

        if (!_useTime)
            return;


        StartCoroutine(ObjectTriggerCycle());
    }
}