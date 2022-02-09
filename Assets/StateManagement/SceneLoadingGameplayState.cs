using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Implementer of IGameplayState for states that load additional scenes.
/// </summary>
public abstract class SceneLoadingGameplayState : IGameplayState
{
    /// <summary>
    /// The name of the Scene that this state loads.
    /// </summary>
    public abstract string SceneName { get; }

    public IEnumerator Load()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator TransitionDown()
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(SceneName);
        while (!unloadScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public virtual IEnumerator TransitionInto()
    {
        // TODO: Animated transition system
        yield break;
    }

    public virtual IEnumerator TransitionUp()
    {
        // TODO: Set everything in scene to inactive? Hide somehow?
        yield break;
    }
}