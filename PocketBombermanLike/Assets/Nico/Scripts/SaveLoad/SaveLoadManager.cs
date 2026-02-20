using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SaveLoadManager : MonoBehaviour
{
    private List<ISaveLoad> _saveLoadObjects = new List<ISaveLoad>();

    private static SaveLoadManager _instance;
    public static SaveLoadManager Instance { get => _instance; }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Subscribe(ISaveLoad obj)
    {
        _saveLoadObjects.Add(obj);
    }

    public void Unsubscribe(ISaveLoad obj)
    {
        _saveLoadObjects.Remove(obj);
    }

    public void Save()
    {
        SaveData data = new SaveData();

        foreach(ISaveLoad obj in _saveLoadObjects)
        {
            obj.SaveCallback(data);
        }

        bool result = SaveLoadSystem.Save(data);

        if(result)
            Debug.Log("[SAVE LOAD MANAGER] SaveData saved successfully.");
        else
            Debug.LogWarning("[SAVE LOAD MANAGER] SaveData saved unsuccessfully.");

    }

    public void Load()
    {
        SaveData data = SaveLoadSystem.Load();

        if (data == null)
        {
            Debug.LogWarning("[SAVE LOAD MANAGER] SaveData loaded unsuccessfully.");
            return;
        }

        foreach (ISaveLoad obj in _saveLoadObjects)
        {
            obj.LoadCallback(data);
        }

        Debug.Log("[SAVE LOAD MANAGER] SaveData loaded successfully.");

    }
}
