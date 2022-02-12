using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthSceneTools : SceneBootstrapperTools
{
    public void ToTheTown()
    {
        ActiveBootstrapper.StartCoroutine(ActiveStateMachine.ChangeToState(new TownState()));
    }

    public void ToBattle()
    {
        ActiveBootstrapper.StartCoroutine(ActiveStateMachine.PushNewState(new BattleState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new LabyrinthState();
    }
}
