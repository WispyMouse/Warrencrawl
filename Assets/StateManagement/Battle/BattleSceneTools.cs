using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneTools : SceneBootstrapperTools
{
    public void EndBattle()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.EndCurrentState());
    }

    public override IGameplayState GetNewDemoState()
    {
        return new BattleState();
    }
}
