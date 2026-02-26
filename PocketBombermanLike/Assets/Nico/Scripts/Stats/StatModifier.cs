using UnityEngine;

public class StatModifier : ScriptableObject
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

    public Type GetType { get => _type; }
    public int GetAmount { get => _amount; }

    public void OnTriggerExit2D(Collider2D collision)
    {

    }
}
