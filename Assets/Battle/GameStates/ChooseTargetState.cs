using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTargetState : CombatGameState
{
    ChooseCommandForAllyState ChooseCommandForAllyInstance { get; set; }
    CombatMember ActingAlly { get; set; }

    public ChooseTargetState(BattleState baseBattleState, ChooseCommandForAllyState allyCommandState, CombatMember actingAlly) : base(baseBattleState)
    {
        ChooseCommandForAllyInstance = allyCommandState;
        ActingAlly = actingAlly;
    }

    public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        int index = 0;
        foreach (CombatMember opponent in BaseBattleState.Opponents.OpposingMembers)
        {
            BattleMenuInstance.AddMenuItem(opponent.DisplayName, TargetChosen(stateMachine, index));
            index++;
        }

        yield break;
    }

    IEnumerator TargetChosen(GlobalStateMachine stateMachine, int index)
    {
        ChooseCommandForAllyInstance.ChosenCommand = new BattleCommand(ActingAlly, BaseBattleState.Opponents.OpposingMembers[index]);
        yield return stateMachine.EndCurrentState();
    }
}
