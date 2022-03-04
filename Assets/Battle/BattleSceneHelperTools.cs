using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneHelperTools : SceneHelperTools
{
    public void EndBattle()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.EndCurrentState());
    }

    public override IGameplayState GetNewDemoState()
    {
        return new BattleState();
    }
}