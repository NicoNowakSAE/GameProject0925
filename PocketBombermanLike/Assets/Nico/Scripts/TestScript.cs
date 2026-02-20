using UnityEngine;

public class TestScript : MonoBehaviour, ISaveLoad
{
    [SerializeField] private int _currentLevel;
    void Start()
    {
        SubscribeToSaveLoadManager();
    }

    public void SubscribeToSaveLoadManager()
    {
        SaveLoadManager.Instance.Subscribe(this);
    }

    public void UnsubscribeToSaveLoadManager()
    {
        SaveLoadManager.Instance.Unsubscribe(this);
    }

    public void LoadCallback(SaveData data)
    {
        _currentLevel = data.CurrentLevel;
    }

    public void SaveCallback(SaveData data)
    {
        data.CurrentLevel = _currentLevel;
    }
}
