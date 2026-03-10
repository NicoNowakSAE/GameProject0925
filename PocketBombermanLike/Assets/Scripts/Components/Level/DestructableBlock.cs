using UnityEngine;
using UnityEngine.Events;

public class DestructableBlock : MonoBehaviour, IBombHit
{
    [SerializeField] private GameObject _powerupPrefab;
    [SerializeField] private PowerupConfig _powerupConfiguration;
    [SerializeField] private bool _spawnPowerupOnDestroy = false;

    private Transform _transform;
    public UnityEvent OnDestroy;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    /// <summary>
    /// Spawns the <see cref="_powerupPrefab"/> at the block's position 
    /// and initializes it with the <see cref="_powerupConfiguration"/>.
    /// </summary>
    private void InstantiatePowerup()
    {
        Debug.Log($"[DESTRUCTABLE BLOCK] Spawning powerup for type: {_powerupConfiguration.Type} -");

        GameObject powerUp = Instantiate(_powerupPrefab, _transform.position, Quaternion.identity);
        StatModifier powerUpStatModifier = powerUp.GetComponent<StatModifier>();

        if (powerUpStatModifier == null)
            return;
        
        powerUpStatModifier.SetConfig(_powerupConfiguration);
    }

    /// <summary>
    /// Implementation of <see cref="IBombHit"/>. Triggers the <see cref="OnDestroy"/> event, 
    /// conditionally spawns a powerup via <see cref="InstantiatePowerup"/>, and removes 
    /// the object from the scene.
    /// </summary>
    /// <param name="dmg">The amount of damage received.</param>
    public void Hit(int dmg)
    {
        Debug.Log($"[DESTRUCTABLE BLOCK] Invoking OnDestroy() -");

        OnDestroy.Invoke();

        if (_spawnPowerupOnDestroy)
            InstantiatePowerup();
            
        Destroy(this.gameObject);
    }
}