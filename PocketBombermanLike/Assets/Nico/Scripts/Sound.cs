using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObjects/Sound", fileName = "New Sound")]
public class Sound : ScriptableObject
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume = 0.5f;
    [Range(-3f, 3f)] public float Pitch = 1;
}
