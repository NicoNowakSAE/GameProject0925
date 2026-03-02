using System.Collections.Generic;
using UnityEngine;

public class HealthCollection : MonoBehaviour
{
    private static Dictionary<GameObject, Health> _gameObjectHealthPairs = new Dictionary<GameObject, Health>();
    public static Dictionary<GameObject, Health> GameObjectHealthPairs => _gameObjectHealthPairs;

    public static void Subscribe(GameObject gameObject, Health health)
    {
        if (_gameObjectHealthPairs.ContainsKey(gameObject))
        {
            Debug.LogWarning($"[HEALTH COLLECTION] Object {gameObject.name} already exists inside the health collection. This can be caused due to having more than one health component on the GameObject. -");
            return;
        }
        
        _gameObjectHealthPairs.Add(gameObject, health);
        Debug.Log($"[HEALTH COLLECTION] Added 1 new Health reference: (GameObject: {gameObject.name}) -");
    }
}
