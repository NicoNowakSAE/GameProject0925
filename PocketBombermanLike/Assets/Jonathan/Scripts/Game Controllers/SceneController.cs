using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Central controller responsible for scene discovery and scene loading.
/// Persists across scene changes.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Currently active scene.
    /// </summary>
    public Scene CurrentScene => SceneManager.GetActiveScene();

    /// <summary>
    /// Total number of scenes registered in the Build Settings.
    /// </summary>
    public int SceneCount => SceneManager.sceneCountInBuildSettings;

    /// <summary>
    /// List of all scene paths defined in the Build Settings.
    /// </summary>
    public List<string> ScenePathList => GetAllScenes();

    /// <summary>
    /// Fired right before a scene load is triggered.
    /// </summary>
    [Header("Scene Events")]
    public UnityEvent OnSceneLoadRequest;

    /// <summary>
    /// Fired after a scene has fully finished loading.
    /// </summary>
    public UnityEvent OnSceneLoadFinished;

    /// <summary>
    /// Collects all scene paths from the Build Settings by build index.
    /// </summary>
    /// <returns>
    /// A list of scene paths as defined in the Build Settings.
    /// </returns>
    private List<string> GetAllScenes()
    {
        Debug.Log($"[SCENE CONTROLLER] Collecting all scene paths, count={SceneCount} -");

        List<string> sceneList = new List<string>();

        for (int i = 0; i < SceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogWarning($"[SCENE CONTROLLER] Scene path empty at index={i} -");
                continue;
            }

            Debug.Log($"[SCENE CONTROLLER] Scene fetched, index={i}, path={scenePath} -");
            sceneList.Add(scenePath);
        }

        Debug.Log($"[SCENE CONTROLLER] Scene collection finished, total={sceneList.Count} -");
        return sceneList;
    }

    /// <summary>
    /// Loads a scene by build index.
    /// </summary>
    /// <param name="index">Build index of the scene.</param>
    /// <param name="mode">Scene loading mode.</param>
    public void LoadScene(int index, LoadSceneMode mode = LoadSceneMode.Single)
    {
        Debug.Log($"[SCENE CONTROLLER] LoadScene(int) invoked, index={index}, mode={mode} -");

        if (index < 0 || index >= SceneCount)
        {
            Debug.LogError($"[SCENE CONTROLLER] Invalid scene index={index}, aborting load -");
            return;
        }

        Debug.Log($"[SCENE CONTROLLER] Scene index validated, invoking load request -");

        OnSceneLoadRequest?.Invoke();
        SceneManager.LoadScene(index, mode);
    }

    /// <summary>
    /// Loads a scene by its name (file name without extension).
    /// </summary>
    /// <param name="name">Name of the scene.</param>
    /// <param name="mode">Scene loading mode.</param>
    public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
    {
        Debug.Log($"[SCENE CONTROLLER] LoadScene(string) invoked, name='{name}', mode={mode} -");

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError($"[SCENE CONTROLLER] Scene name is null or empty, aborting load -");
            return;
        }

        string[] sceneNameList = ScenePathList
            .Select(scene => Path.GetFileNameWithoutExtension(scene))
            .ToArray();

        Debug.Log($"[SCENE CONTROLLER] Available scenes: [{string.Join(", ", sceneNameList)}] -");

        if (!sceneNameList.Contains(name))
        {
            Debug.LogError($"[SCENE CONTROLLER] Scene '{name}' not found in build settings -");
            return;
        }

        Debug.Log($"[SCENE CONTROLLER] Scene name validated, invoking load request -");

        OnSceneLoadRequest?.Invoke();
        SceneManager.LoadScene(name, mode);
    }

    /// <summary>
    /// Loads the next scene based on the current scene's build index.
    /// </summary>
    /// <param name="mode">Scene loading mode.</param>
    public void LoadNextScene(LoadSceneMode mode = LoadSceneMode.Single)
    {
        int nextIndex = CurrentScene.buildIndex + 1;

        Debug.Log(
            $"[SCENE CONTROLLER] LoadNextScene invoked, " +
            $"currentIndex={CurrentScene.buildIndex}, nextIndex={nextIndex}, mode={mode} -"
        );

        LoadScene(nextIndex, mode);
    }

    /// <summary>
    /// Loads the previous scene based on the current scene's build index.
    /// </summary>
    /// <param name="mode">Scene loading mode.</param>
    public void LoadPreviousScene(LoadSceneMode mode = LoadSceneMode.Single)
    {
        int previousIndex = CurrentScene.buildIndex - 1;

        Debug.Log(
            $"[SCENE CONTROLLER] LoadPreviousScene invoked, " +
            $"currentIndex={CurrentScene.buildIndex}, previousIndex={previousIndex}, mode={mode} -"
        );

        LoadScene(previousIndex, mode);
    }

    /// <summary>
    /// Registers scene load callback and marks this object as persistent.
    /// </summary>
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Callback invoked by Unity after a scene has finished loading.
    /// </summary>
    /// <param name="scene">Loaded scene.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[SCENE CONTROLLER] Scene loaded successfully, name='{scene.name}' -");

        OnSceneLoadFinished?.Invoke();
    }
}
