using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// The global entry point for regular gameplay of this engine.
/// This object exists in a scene and bootstraps the code and gets things going.
/// </summary>
public class InitialSceneMartial : MonoBehaviour
{
    public GlobalStateMachine GlobalStateMachineInstance { get; set; }

    void Start()
    {
        StartCoroutine(ProcessStart());
    }

    IEnumerator ProcessStart()
    {
        AsyncOperation loadInputScene = SceneManager.LoadSceneAsync("Input", LoadSceneMode.Additive);
        while (!loadInputScene.isDone)
        {
            yield return null;
        }

        GlobalStateMachineInstance = new GlobalStateMachine();

        PlayerInput activePlayerInput = GameObject.FindObjectOfType<PlayerInput>();
        activePlayerInput.onControlsChanged += GlobalStateMachineInstance.SetControls;

        MainMenuState mainMenuState = new MainMenuState();
        StartCoroutine(GlobalStateMachineInstance.ChangeToState(mainMenuState));
    }
}
