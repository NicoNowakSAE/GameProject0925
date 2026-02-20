using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveLoadManager))]
public class SaveLoadManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SaveLoadManager manager = (SaveLoadManager)target;

        DrawDefaultInspector();

        bool pressed = GUILayout.Button("Save");
        if (pressed)
        {
            manager.Save();
        }

        pressed = GUILayout.Button("Load");
        if (pressed)
        {
            manager.Load();
        }

    }
}
