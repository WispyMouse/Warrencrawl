using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneTools : SceneBootstrapperTools
{
    public void ToTheTown()
    {
        ActiveBootstrapper.StartCoroutine(ActiveStateMachine.ChangeToState(new TownState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new MainMenuState();
    }
}
