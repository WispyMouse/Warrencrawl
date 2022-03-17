using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCommandForAllyState : CombatGameState
{
    CombatMember ActingAlly { get; set; }
    public BattleCommand ChosenCommand { get; set; }

    public ChooseCommandForAllyState(BattleState baseBattleState, CombatMember forMember) : base(baseBattleState)
    {
        ActingAlly = forMember;
    }

    public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        if (previousState is ChooseTargetState)
        {
            BaseBattleState.BattleCommands.Add(ChosenCommand);
            yield return stateMachine.EndCurrentState();
            yield break;
        }

        BattleMenuInstance.AddMenuItem("Attack", AttackChosen(stateMachine));
        BattleMenuInstance.AddMenuItem("Escape", EscapeChosen(stateMachine));
        yield break;
    }

    IEnumerator AttackChosen(GlobalStateMachine stateMachine)
    {
        yield return stateMachine.PushNewState(new ChooseTargetState(BaseBattleState, this, ActingAlly));
    }

    IEnumerator EscapeChosen(GlobalStateMachine stateMachine)
    {
        ChosenCommand = new BattleCommand(ActingAlly, null);
        BaseBattleState.BattleCommands.Add(ChosenCommand);
        yield return stateMachine.EndCurrentState();
    }
}
