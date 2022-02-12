using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneTools : SceneBootstrapperTools
{
    public void ToTheTown()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.ChangeToState(new TownState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new MainMenuState();
    }
}
