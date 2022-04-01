using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides a wrapper for yieldable <see cref="SceneManager"/> operations.
/// </summary>
public static class StaticSceneTools
{
    /// <summary>
    /// Returns if a scene with a given name is currently loaded.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns>True if the scene is currently loaded.</returns>
    public static bool IsSceneLoaded(string name)
    {
        for (int ii = 0; ii < SceneManager.sceneCount; ii++)
        {
            if (SceneManager.GetSceneAt(ii).name == name)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Loads a scene additively (in addition to scenes already loaded).
    /// This looks to the bundled scenes; it won't reach in to Addressables.
    /// Generally used for major state machine states.
    /// </summary>
    /// <param name="name">The name of the scene to load.</param>
    public static IEnumerator LoadSceneAdditvely(string name)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return loadScene.progress;
        }
    }

    /// <summary>
    /// Loads an addressable scene additively (in addition to scenes already loaded).
    /// This only looks at scenes that are marked as Addressable.
    /// Generally used for labyrinth scenes.
    /// </summary>
    /// <param name="level">The GameLevel to load the scene for.</param>
    public static IEnumerator LoadAddressableSceneAdditvely(GameLevel level)
    {
        AsyncOperationHandle<SceneInstance> loadingOperation = Addressables.LoadSceneAsync(level.Scene, LoadSceneMode.Additive);
        yield return loadingOperation;
    }

    /// <summary>
    /// Unloads a scene with the provided name.
    /// This can unload either Addressable or standard scenes.
    /// </summary>
    /// <param name="name">The name of the scene to unload.</param>
    public static IEnumerator UnloadScene(string name)
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(name);

        if (unloadScene == null)
        {
            Debug.LogError($"Could not find scene to exit: {name}");
        }

        while (!unloadScene.isDone)
        {
            yield return unloadScene.progress;
        }
    }
}
