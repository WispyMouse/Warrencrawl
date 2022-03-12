using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCommandForAllyState : IGameplayState
{
    ChooseCommandsForPartyState PartyCommandState { get; set; }
    GlobalStateMachine StateMachineInstance { get; set; }
    CombatMember ActingAlly { get; set; }

    public ChooseCommandForAllyState(ChooseCommandsForPartyState partyCommandState, GlobalStateMachine stateMachine, CombatMember forMember)
    {
        PartyCommandState = partyCommandState;
        StateMachineInstance = stateMachine;
        ActingAlly = forMember;
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
        Debug.Log(ActingAlly.GetType().Name);
        PartyCommandState.BattleCommands.Add(new BattleCommand());
        yield return stateMachine.EndCurrentState();
    }
}
