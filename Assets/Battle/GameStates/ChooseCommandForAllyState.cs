using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCommandForAllyState : IGameplayState
{
    ChooseCommandsForPartyState PartyCommandState { get; set; }
    GlobalStateMachine StateMachineInstance { get; set; }
    CombatMember ActingAlly { get; set; }
    BattleCommand ChosenCommand { get; set; }

    public BattleMenu BattleMenuInstance { get; set; }

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
        BattleMenuInstance.AddMenuItem("Attack", AttackChosen);
        BattleMenuInstance.AddMenuItem("Escape", EscapeChosen);
        yield break;
    }

    IEnumerator AttackChosen()
    {
        ChosenCommand = new BattleCommand();
        PartyCommandState.BattleCommands.Add(ChosenCommand);
        yield return StateMachineInstance.EndCurrentState();
    }

    IEnumerator EscapeChosen()
    {
        ChosenCommand = new BattleCommand();
        PartyCommandState.BattleCommands.Add(ChosenCommand);
        yield return StateMachineInstance.EndCurrentState();
    }
}
