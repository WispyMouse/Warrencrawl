using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTargetState : IGameplayState
{
    public BattleMenu BattleMenuInstance { get; set; }
    GlobalStateMachine StateMachineInstance { get; set; }
    ChooseCommandForAllyState ChooseCommandForAllyInstance { get; set; }
    BattleState BattleStateInstance { get; set; }

    public ChooseTargetState(GlobalStateMachine stateMachineInstance, ChooseCommandForAllyState allyCommandState, BattleState battleStateInstance)
    {
        StateMachineInstance = stateMachineInstance;
        ChooseCommandForAllyInstance = allyCommandState;
        BattleStateInstance = battleStateInstance;
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
        int index = 0;
        foreach (CombatMember opponent in BattleStateInstance.Opponents.OpposingMembers)
        {
            int indexHolder = index;
            BattleMenuInstance.AddMenuItem(opponent.GetType().ToString(), TargetChosen(index));
            index++;
        }

        yield break;
    }

    IEnumerator TargetChosen(int index)
    {
        yield return StateMachineInstance.EndCurrentState();
    }
}
