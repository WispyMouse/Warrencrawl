using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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

    /// <summary>
    /// The names of the Action Maps to enable in a PlayerInput.
    /// </summary>
    public abstract string[] InputMapNames { get; }

    public IEnumerator Load()
    {
        if (!SceneManager.GetAllScenes().Any(sc => sc.name == SceneName))
        {
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            while (!loadScene.isDone)
            {
                yield return loadScene.progress;
            }
        }
    }
    public IEnumerator ExitState(IGameplayState nextState)
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(SceneName);
        while (!unloadScene.isDone)
        {
            yield return unloadScene.progress;
        }
    }

    public virtual IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        // TODO: Animated transition system

        foreach (GameObject rootObj in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            rootObj.SetActive(true);
        }

        yield break;
    }

    public virtual IEnumerator TransitionUp(IGameplayState nextState)
    {
        foreach(GameObject rootObj in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            rootObj.SetActive(false);
        }

        yield break;
    }

    public virtual void SetControls(PlayerInput controls)
    {
        if (controls == null)
        {
            return;
        }    

        controls.actions.Disable();
        foreach (string inputMapName in InputMapNames)
        {
            controls.actions.FindActionMap(inputMapName).Enable();
        }
    }
}
