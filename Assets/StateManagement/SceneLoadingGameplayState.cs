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
        if (!StaticSceneTools.IsSceneLoaded(SceneName))
        {
            yield return StaticSceneTools.LoadSceneAdditvely(SceneName);
        }

        foreach (GameObject rootObj in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            rootObj.SetActive(true);
        }

        SceneHelperInstance = GameObject.FindObjectOfType<SceneHelper>();
    }

    public virtual IEnumerator AnimateTransitionOut(IGameplayState nextState)
    {
        if (nextState == null)
        {
            yield break;
        }

        yield return SceneHelperInstance.TransitionsInstance.StartTransition();
    }

    public virtual IEnumerator ExitState(IGameplayState nextState)
    {
        yield return StaticSceneTools.UnloadScene(SceneName);
    }

    public virtual IEnumerator AnimateTransitionIn(IGameplayState previousState)
    {
        if (previousState == null)
        {
            yield break;
        }

        yield return SceneHelperInstance.TransitionsInstance.FinishTransitionYieldUntilInputsOK();
    }

    public virtual IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        StateMachineInstance = globalStateMachine;

        yield break;
    }

    public virtual IEnumerator ChangeUp(IGameplayState nextState)
    {
        foreach(GameObject rootObj in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            rootObj.SetActive(false);
        }

        yield break;
    }

    public abstract void SetControls(WarrencrawlInputs controls);
}
