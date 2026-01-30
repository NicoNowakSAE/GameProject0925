using UnityEngine;

public abstract class EntityBaseBehaviour : MonoBehaviour
{
    public abstract void Tick();
    public abstract void FixedTick();
}