using UnityEngine;

public abstract class Triggerable : MonoBehaviour
{
    private bool _isActive;

    /// <summary>
    /// Gets the current activation status of the object.
    /// </summary>
    public bool IsActive => _isActive;

    private bool _hasBeenUsedBefore = false;

    /// <summary>
    /// Initiates the activation sequence. If <see cref="_isActive"/> is already true, the call is ignored.
    /// Sets <see cref="_isActive"/> to true and triggers <see cref="OnActivate"/>.
    /// </summary>
    public void Activate()
    {
        if (_hasBeenUsedBefore && _isActive) 
            return;

        if (!_hasBeenUsedBefore) 
            _hasBeenUsedBefore = true;

        _isActive = true;

        OnActivate();
    }

    /// <summary>
    /// Initiates the deactivation sequence. If <see cref="IsActive"/> is already false, the call is ignored.
    /// Sets <see cref="_isActive"/> to false and triggers <see cref="OnDeactivate"/>.
    /// </summary>
    public void Deactivate()
    {
        if (_hasBeenUsedBefore && !_isActive)
            return;

        if (!_hasBeenUsedBefore) 
            _hasBeenUsedBefore = true;

        _isActive = false;

        OnDeactivate();
    }

    /// <summary>
    /// Abstract hook for defining custom logic that occurs when the object is deactivated.
    /// </summary>
    protected abstract void OnDeactivate();

    /// <summary>
    /// Abstract hook for defining custom logic that occurs when the object is activated.
    /// </summary>
    protected abstract void OnActivate();
}