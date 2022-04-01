using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The parent class for various combat focused gameplaystates.
/// Helps by closing out of the various combat UX.
/// </summary>
public abstract class CombatGameState : IGameplayState
{
    protected readonly BattleState BaseBattleState;

    protected BattleMenu BattleMenuInstance { get; set; }

    public CombatGameState(BattleState baseBattleState)
    {
        this.BaseBattleState = baseBattleState;
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

    public void UnsetControls(WarrencrawlInputs inputs)
    {
        
    }

    public abstract IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState);
}
