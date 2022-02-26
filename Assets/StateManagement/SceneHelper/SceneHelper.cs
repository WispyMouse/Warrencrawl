using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// The global entry point for regular gameplay of this engine.
/// This object exists in a scene and bootstraps the code and gets things going.
/// If a scene with just this is loaded, it'll get the game started on the MainMenu.
/// If another scene is open when this is loaded, this'll try to load the game to an appropriate state.
/// </summary>
public class SceneHelper : MonoBehaviour
{
    public GlobalStateMachine GlobalStateMachineInstance { get; set; }
    private WarrencrawlInputs Inputs { get; set; }

    private void Start()
    {
        Inputs = new WarrencrawlInputs();
        GlobalStateMachineInstance = new GlobalStateMachine(Inputs);

        SceneHelperTools bootstrapperTools = GameObject.FindObjectOfType<SceneHelperTools>();

        if (bootstrapperTools == null)
        {
            MainMenuState mainMenuState = new MainMenuState();
            StartCoroutine(GlobalStateMachineInstance.ChangeToState(mainMenuState));
        }
        else
        {
            IGameplayState firstState = bootstrapperTools.GetNewDemoState();
            StartCoroutine(GlobalStateMachineInstance.ChangeToState(firstState));
        }
    }
}
