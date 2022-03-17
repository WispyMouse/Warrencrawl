using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveCommandsState : CombatGameState
{
    public List<BattleCommand> AllBattleCommands { get; set; }

    public ResolveCommandsState(BattleState baseBattleState, List<BattleCommand> allBattleCommands):base(baseBattleState)
    {
        AllBattleCommands = allBattleCommands;
    }

    public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        foreach (BattleCommand command in AllBattleCommands)
        {
            Debug.Log("Resolving command...");
            Debug.Log($"{command.ActingMember.DisplayName} targets {command.Target?.DisplayName}");
        }

        yield return stateMachine.EndCurrentState();
    }
}
