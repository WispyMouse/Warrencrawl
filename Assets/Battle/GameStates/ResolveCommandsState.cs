using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveCommandsState : IGameplayState
{
    public BattleState BattleStateInstance { get; set; }
    public List<BattleCommand> AllBattleCommands { get; set; }

    public ResolveCommandsState(BattleState battleState, List<BattleCommand> allBattleCommands)
    {
        BattleStateInstance = battleState;
        AllBattleCommands = allBattleCommands;
    }

    public IEnumerator AnimateTransitionIn(IGameplayState previousState)
    {
        yield break;
    }

    public IEnumerator AnimateTransitionOut(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator ChangeUp(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator Load()
    {
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {
        
    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        foreach (BattleCommand command in AllBattleCommands)
        {
            Debug.Log("Resolving command...");
            Debug.Log($"{command.ActingMember.DisplayName} targets {command.Target?.DisplayName}");
        }

        yield return stateMachine.EndCurrentState();
    }
}
