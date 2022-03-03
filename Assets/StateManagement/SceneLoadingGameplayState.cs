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

    public GlobalStateMachine StateMachineInstance { get; private set; }

    public SceneHelper SceneHelperInstance { get; private set; }

    public virtual IEnumerator Load()
    {
        if (!SceneManager.GetAllScenes().Any(sc => sc.name == SceneName))
        {
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            while (!loadScene.isDone)
            {
                yield return loadScene.progress;
            }
        }

        foreach (GameObject rootObj in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            rootObj.SetActive(true);
        }

        SceneHelperInstance = GameObject.FindObjectOfType<SceneHelper>();
    }

    public virtual IEnumerator ExitState(IGameplayState nextState)
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(SceneName);

        if (unloadScene == null)
        {
            Debug.Log($"Could not find scene to exit: {GetType()}");
        }

        while (!unloadScene.isDone)
        {
            yield return unloadScene.progress;
        }
    }

    public virtual IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        // TODO: Animated transition system
        StateMachineInstance = globalStateMachine;

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

    public abstract void SetControls(WarrencrawlInputs controls);
}
