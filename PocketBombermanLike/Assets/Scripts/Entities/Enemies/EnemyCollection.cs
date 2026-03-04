using System.Collections.Generic;
using UnityEngine;

public class EnemyCollection : MonoBehaviour
{
    private static List<GameObject> _enemyList = new List<GameObject>();
    public static List<GameObject> EnemyList => _enemyList;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        Debug.Log("[ENEMY COLLECTION] Initializing enemy collection... -");
        _enemyList = new List<GameObject>();
    }

    public static void Subscribe(GameObject gameObject)
    {
        _enemyList.RemoveAll(obj => obj == null);

        if (_enemyList.Contains(gameObject))
        {
            Debug.LogWarning($"[ENEMY COLLECTION] Object {gameObject.name} already exists inside the enemy collection and therefore won't be added to avoid duplicates. If this behaviour isn't intended, check the code! -");
            return;
        }
        _enemyList.Add(gameObject);
        Debug.Log($"[ENEMY COLLECTION] Added 1 new Enemy reference: (GameObject: {gameObject.name}) -");
    }

    public static void Unsubscribe(GameObject gameObject)
    {
        if (!_enemyList.Contains(gameObject))
            return;
            
        _enemyList.Remove(gameObject);
        Debug.Log($"[ENEMY COLLECTION] Removed 1 Enemy reference: (GameObject: {gameObject.name}) -");
    }


}
