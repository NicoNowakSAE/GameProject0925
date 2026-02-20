using UnityEngine;

public interface ISaveLoad
{
    public void SubscribeToSaveLoadManager();
    public void UnsubscribeToSaveLoadManager();

    public void LoadCallback(SaveData data);
    public void SaveCallback(SaveData data);
}
