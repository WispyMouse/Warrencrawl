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
        BattleOpponents opponents = new BattleOpponents();

        opponents.AddOpposingMember(new CombatMember());
        opponents.AddOpposingMember(new CombatMember());
        opponents.AddOpposingMember(new CombatMember());

        opponents.AddOpposingMember(new CombatMember());
        opponents.AddOpposingMember(new CombatMember());
        opponents.AddOpposingMember(new CombatMember());

        return new BattleState(opponents);
    }
}
