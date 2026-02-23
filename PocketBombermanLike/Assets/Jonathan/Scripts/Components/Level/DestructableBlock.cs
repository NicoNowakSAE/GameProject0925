using UnityEngine;
using UnityEngine.Events;

public class DestructableBlock : MonoBehaviour, IBombHit
{
    public UnityEvent OnDestroy;

    public void Hit(int dmg)
    {
        OnDestroy.Invoke();
        Destroy(this.gameObject);
    }
}
