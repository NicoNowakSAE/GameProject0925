using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controls execution of a Goomba-like behaviour.
/// Delegates Update and FixedUpdate calls to the assigned behaviour
/// via the IGoombaLikeBehaviour interface.
/// </summary>
public class GoombaLike_Controller : MonoBehaviour
{
    /// <summary>
    /// Active Goomba-like behaviour attached to this GameObject.
    /// </summary>
    private IGoombaLikeBehaviour _behaviour;

    /// <summary>
    /// Caches the Goomba-like behaviour component.
    /// </summary>
    private void Awake()
    {
        _behaviour = GetComponent<IGoombaLikeBehaviour>();
    }

    /// <summary>
    /// Forwards frame-based logic to the behaviour.
    /// </summary>
    private void Update()
    {
        _behaviour?.Tick();
    }

    /// <summary>
    /// Forwards physics-based logic to the behaviour.
    /// </summary>
    private void FixedUpdate()
    {
        _behaviour?.FixedTick();
    }
}
