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
            // If we were sent to this state, and the ChosenCommand is null, then back
            if (ChosenCommand == null)
            {
                yield return Back(stateMachine);
                yield break;
            }

            BaseBattleState.BattleCommands.Add(ChosenCommand);
            yield return stateMachine.EndCurrentState();
            yield break;
        }

        BattleMenuInstance.AddMenuItem("Attack", AttackChosen(stateMachine));
        BattleMenuInstance.AddMenuItem("Escape", EscapeChosen(stateMachine));

        if (BaseBattleState.BattleCommands.Count > 0)
        {
            BattleMenuInstance.AddMenuItem("Back", Back(stateMachine));
        }

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

    IEnumerator Back(GlobalStateMachine stateMachine)
    {
        // If there are any commands, remove the latest one
        if (BaseBattleState.BattleCommands.Count > 0)
        {
            BaseBattleState.BattleCommands.RemoveAt(BaseBattleState.BattleCommands.Count - 1);
        }

        yield return stateMachine.EndCurrentState();
    }
}
