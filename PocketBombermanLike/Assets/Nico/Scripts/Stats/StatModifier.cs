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

    public Type GetType { get => _type; }
    public int GetAmount { get => _amount; }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == false)
            return;

        PlayerStatsSystem.Instance.AddModifier(this);

        if (_destroyOnTrigger)
            Destroy(this.gameObject);
    }
}
