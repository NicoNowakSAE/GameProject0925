using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using System.Linq;
public class Tools : MonoBehaviour
{
#if UNITY_EDITOR
    private static int enemyLayerMask = LayerMask.NameToLayer("Enemy");

    [MenuItem("Tools/Print Enemy Count")]
    public static void GetEnemyCount()
    {
        GameObject[] objectsInScene = FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);
        GameObject[] enemiesInScene = objectsInScene.Where(e => e.layer == enemyLayerMask).ToArray();

        Debug.Log($"[TOOLS] There are {enemiesInScene.Count()} enemies in the current scene.");
    }

    [MenuItem("Tools/Disable All Enemies")]
    public static void DisableAllEnemies()
    {
        GameObject[] objectsInScene = FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);
        GameObject[] enemiesInScene = objectsInScene.Where(e => e.layer == enemyLayerMask).ToArray();

        foreach (GameObject enemy in enemiesInScene)
        {
            Debug.Log($"[TOOLS] Disabling enemy: {enemy.name} -");
            enemy.SetActive(false); 
        }
    }
#endif
}