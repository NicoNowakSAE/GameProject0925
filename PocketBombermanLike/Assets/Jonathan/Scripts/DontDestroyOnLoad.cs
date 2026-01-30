using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] private SceneController _controller;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void NextScene()
    {
        _controller.LoadNextScene();
    }

    public void PrevScene()
    {
        _controller.LoadPreviousScene();
    }
}
