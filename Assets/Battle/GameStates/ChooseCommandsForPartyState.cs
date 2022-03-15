using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCommandsForPartyState : IGameplayState
{
    BattleState ActiveBattleState { get; set; }
    GlobalStateMachine StateMachineInstance { get; set; }
    int curPartyMemberIndex { get; set; } = 0;
    public List<BattleCommand> BattleCommands { get; set; } = new List<BattleCommand> ();

    public ChooseCommandsForPartyState(BattleState activeBattleState, GlobalStateMachine stateMachine)
    {
        ActiveBattleState = activeBattleState;
        StateMachineInstance = stateMachine;
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
        if (curPartyMemberIndex >= ActiveBattleState.PlayerPartyPointer.PartyMembers.Count)
        {
            ActiveBattleState.BattleCommands = BattleCommands;
            yield return stateMachine.EndCurrentState();
            yield break;
        }

        CombatMember thisPartyMember = ActiveBattleState.PlayerPartyPointer.PartyMembers[curPartyMemberIndex];
        curPartyMemberIndex++;
        yield return stateMachine.PushNewState(new ChooseCommandForAllyState(this, stateMachine, ActiveBattleState, thisPartyMember));
    }
}
