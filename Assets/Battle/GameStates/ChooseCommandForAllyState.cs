using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCommandForAllyState : IGameplayState
{
    ChooseCommandsForPartyState PartyCommandState { get; set; }
    GlobalStateMachine StateMachineInstance { get; set; }
    BattleState ActiveBattleState { get; set; }
    CombatMember ActingAlly { get; set; }
    public BattleCommand ChosenCommand { get; set; }

    public BattleMenu BattleMenuInstance { get; set; }

    public ChooseCommandForAllyState(ChooseCommandsForPartyState partyCommandState, GlobalStateMachine stateMachine, BattleState activeBattleState, CombatMember forMember)
    {
        PartyCommandState = partyCommandState;
        StateMachineInstance = stateMachine;
        ActiveBattleState = activeBattleState;
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
        BattleMenuInstance.ClearItems();
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        BattleMenuInstance.ClearItems();
        yield break;
    }

    public IEnumerator Load()
    {
        BattleMenuInstance = GameObject.FindObjectOfType<BattleMenu>();
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {

    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        if (previousState is ChooseTargetState)
        {
            PartyCommandState.BattleCommands.Add(ChosenCommand);
            yield return stateMachine.EndCurrentState();
            yield break;
        }

        BattleMenuInstance.AddMenuItem("Attack", AttackChosen());
        BattleMenuInstance.AddMenuItem("Escape", EscapeChosen());
        yield break;
    }

    IEnumerator AttackChosen()
    {
        yield return StateMachineInstance.PushNewState(new ChooseTargetState(StateMachineInstance, this, ActiveBattleState, ActingAlly));
    }

    IEnumerator EscapeChosen()
    {
        ChosenCommand = new BattleCommand(ActingAlly, null);
        PartyCommandState.BattleCommands.Add(ChosenCommand);
        yield return StateMachineInstance.EndCurrentState();
    }
}
