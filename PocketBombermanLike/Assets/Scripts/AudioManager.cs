using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private int _channelCount;
    private AudioSource[] _channels;
    [SerializeField] private Sound[] _sounds;

    public static AudioManager Instance;

    private void Awake()
    {
        // TODO: Singleton?
        Instance = this;

        _sounds = Resources.LoadAll<Sound>("ScriptableObjects");

        _channels = new AudioSource[_channelCount];
        for (int i = 0; i < _channelCount; i++)
        {
            _channels[i] = this.gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string sound)
    {
        for(int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].name == sound)
            {
                Play(_sounds[i]);
                return;
            }
        }
    }

    public void PlaySound(Sound sfx)
    {
        for(int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i] == sfx)
            {
                Play(_sounds[i]);
                return;
            }
        }
    }

    private void Play(Sound sfx)
    {
        for (int i = 0; i < _channels.Length; i++)
        {
            if (_channels[i].isPlaying == false)
            {
                _channels[i].clip = sfx.Clip;
                _channels[i].volume = sfx.Volume;
                _channels[i].pitch = sfx.Pitch;
                _channels[i].Play();
                return;
            }
        }
    }
}
