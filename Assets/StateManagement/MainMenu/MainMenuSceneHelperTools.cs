using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneHelperTools : SceneHelperTools
{
    public void ToTheTown()
    {
        SceneHelperInstance.PlayCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new TownState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new MainMenuState();
    }
}
