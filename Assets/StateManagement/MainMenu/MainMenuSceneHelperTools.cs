using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneHelperTools : SceneHelperTools
{
    public void ToTheTown()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new TownState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new MainMenuState();
    }
}
