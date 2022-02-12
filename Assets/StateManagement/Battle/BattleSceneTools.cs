using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneTools : SceneBootstrapperTools
{
    public void EndBattle()
    {
        ActiveBootstrapper.StartCoroutine(ActiveStateMachine.EndCurrentState());
    }

    public override IGameplayState GetNewDemoState()
    {
        return new BattleState();
    }
}
