using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 2);
    }

}
