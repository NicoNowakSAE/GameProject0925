using UnityEngine;

public class StatModifier : MonoBehaviour
{
    public enum Type
    {
        None,
        BombCount,
        BombRange,
        MoveSpeed,
    }

    [SerializeField] private Type _type;

    [SerializeField] private int _amount = 1;
    [SerializeField] private bool _destroyOnTrigger = true;
    [SerializeField] private LayerMask _collisionLayer;

    public Type GetType { get => _type; }
    public int GetAmount { get => _amount; }

    public void SetConfig(PowerupConfig config)
    {
        Debug.Log($"[STAT MODIFIER] Setting data: \n{config.ToString()} -");
        
        _type = config.Type;
        _amount = config.Amount;
        _destroyOnTrigger = config.DestroyOnTrigger;
        _collisionLayer = config.CollisionLayer;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_collisionLayer.value & (1 << collision.gameObject.layer)) == 0)
            return;

        PlayerStatsSystem.Instance.AddModifier(this);

        if (_destroyOnTrigger)
            Destroy(this.gameObject);
    }
}
