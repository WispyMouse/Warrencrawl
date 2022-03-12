using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCommandsForPartyState : IGameplayState
{
    BattleState ActiveBattleState { get; set; }
    GlobalStateMachine StateMachineInstance { get; set; }
    int curPartyMemberIndex { get; set; } = 0;
    PlayerParty playerPartyPointer { get; set; }
    public List<BattleCommand> BattleCommands { get; set; } = new List<BattleCommand> ();

    public ChooseCommandsForPartyState(BattleState activeBattleState, GlobalStateMachine stateMachine, PlayerParty PlayerPartyPointer)
    {
        ActiveBattleState = activeBattleState;
        StateMachineInstance = stateMachine;
        playerPartyPointer = PlayerPartyPointer;
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
        if (curPartyMemberIndex >= playerPartyPointer.PartyMembers.Count)
        {
            
            yield return stateMachine.EndCurrentState();
            yield break;
        }

        CombatMember thisPartyMember = playerPartyPointer.PartyMembers[curPartyMemberIndex];
        curPartyMemberIndex++;
        yield return stateMachine.PushNewState(new ChooseCommandForAllyState(this, stateMachine, thisPartyMember));
    }
}
