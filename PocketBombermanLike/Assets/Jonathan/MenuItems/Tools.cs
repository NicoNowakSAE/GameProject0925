using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using System.Linq;
public class Tools : MonoBehaviour
{
#if UNITY_EDITOR

    [MenuItem("Tools/Print Enemy Count")]
    public static void GetEnemyCount()
    {
        int enemyLayerMask = LayerMask.NameToLayer("Enemy");

        GameObject[] objectsInScene = FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);
        GameObject[] enemiesInScene = objectsInScene.Where(e => e.layer == enemyLayerMask).Where(e => e.transform.parent == null).ToArray();

        Debug.Log($"[TOOLS] There are {enemiesInScene.Count()} enemies in the current scene.");
    }
#endif
}