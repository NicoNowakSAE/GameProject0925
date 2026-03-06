using UnityEngine;

public class StartupManager : MonoBehaviour
{
    private void Start()
    {
        SaveLoadManager.Instance.Load();
    }

}
