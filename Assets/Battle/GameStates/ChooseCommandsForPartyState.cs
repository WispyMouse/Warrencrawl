using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCommandsForPartyState : CombatGameState
{
    int curPartyMemberIndex { get; set; } = 0;

    public ChooseCommandsForPartyState(BattleState baseBattleState) : base(baseBattleState)
    {
    }

    public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        if (curPartyMemberIndex >= BaseBattleState.PlayerPartyPointer.PartyMembers.Count)
        {
            yield return stateMachine.EndCurrentState();
            yield break;
        }

        CombatMember thisPartyMember = BaseBattleState.PlayerPartyPointer.PartyMembers[curPartyMemberIndex];
        curPartyMemberIndex++;
        yield return stateMachine.PushNewState(new ChooseCommandForAllyState(BaseBattleState, thisPartyMember));
    }
}
