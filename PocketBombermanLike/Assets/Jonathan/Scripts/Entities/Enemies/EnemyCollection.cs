using System.Collections.Generic;
using UnityEngine;

public class EnemyCollection : MonoBehaviour
{
    private static List<GameObject> _enemyList = new List<GameObject>();
    public static List<GameObject> EnemyList => _enemyList;

    public static void Subscribe(GameObject gameObject)
    {
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
