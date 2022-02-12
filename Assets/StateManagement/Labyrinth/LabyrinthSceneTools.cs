using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthSceneTools : SceneBootstrapperTools
{
    public void ToTheTown()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.ChangeToState(new TownState()));
    }

    public void ToBattle()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.PushNewState(new BattleState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new LabyrinthState();
    }
}
