using UnityEngine;

public class HitTest : MonoBehaviour, IBombHit
{
    public void Hit(int dmg)
    {
        Destroy(this.gameObject);
    }

}
