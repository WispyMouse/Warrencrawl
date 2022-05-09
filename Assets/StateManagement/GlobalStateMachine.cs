using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the <see cref="IGameplayState"/>s of the game.
/// If you want to transition from one state to another, that goes through here.
/// Receives input and passes it to the current active state.
/// </summary>
public class GlobalStateMachine
{
    /// <summary>
    /// Cached reference to the last set of controls used.
    /// </summary>
    WarrencrawlInputs lastActiveControls { get; set; }

    /// <summary>
    /// The stack of states currently loaded in to memory.
    /// States underneath the top may be "inactive" and out of memory, and the top state may not be fully "active".
    /// </summary>
    Stack<IGameplayState> PresentStates { get; set; } = new Stack<IGameplayState>();

    public IGameplayUXProvider UXProvider { get; private set; }

    private bool InTransitionState { get; set; } = false;

    /// <summary>
    /// Constructor for the GlobalStateMachine.
    /// </summary>
    /// <param name="inputs">The input map used.</param>
    /// <param name="uxProvider">The provider for UX elements for the game.</param>
    public GlobalStateMachine(WarrencrawlInputs inputs, IGameplayUXProvider uxProvider)
    {
        this.lastActiveControls = inputs;
        this.UXProvider = uxProvider;
    }

    /// <summary>
    /// Returns the current top level state, if there is one.
    /// </summary>
    public IGameplayState CurrentState
    {
        get
        {
            if (PresentStates.TryPeek(out IGameplayState currentState))
            {
                return currentState;
            }

            return null;
        }
    }
    
    /// <summary>
    /// Transitions in to a new state, finishing when complete.
    /// The old state is fully unloaded and removed.
    /// </summary>
    /// <param name="newState">The state to transition in to.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator ChangeToState(IGameplayState newState)
    {
        InTransitionState = true;
        IGameplayState oldState = CurrentState;
        if (oldState != null)
        {
            yield return oldState.AnimateTransitionOut(newState, StateLeavingConditions.LeavingState);
            yield return oldState.ExitState(newState, StateLeavingConditions.LeavingState);
            PresentStates.Pop();
        }

        PresentStates.Push(newState);
        yield return WarmUpAndStartCurrentState(oldState);
    }

    public void StartPushNewState(IGameplayState newState)
    {
        UXProvider.CoroutineRunner.PlayCoroutine(PushNewState(newState));
    }

    /// <summary>
    /// Transitions to a new state, finishing when complete.
    /// The old state remains, lower on the <see cref="PresentStates"/> stack.
    /// </summary>
    /// <param name="newState">The state to transition in to.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator PushNewState(IGameplayState newState)
    {
        InTransitionState = true;
        IGameplayState oldState = CurrentState;
        yield return oldState?.AnimateTransitionOut(newState, StateLeavingConditions.PushNewState);
        yield return oldState?.ExitState(newState, StateLeavingConditions.PushNewState);
        PresentStates.Push(newState);
        yield return WarmUpAndStartCurrentState(oldState);
    }

    public void StartEndCurrentState()
    {
        UXProvider.CoroutineRunner.PlayCoroutine(EndCurrentState());
    }

    /// <summary>
    /// Transitions out of the current state to the one below it.
    /// </summary>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator EndCurrentState()
    {
        InTransitionState = true;
        IGameplayState oldState = CurrentState;
        PresentStates.Pop();

        PresentStates.TryPeek(out IGameplayState nextState);
        yield return oldState?.AnimateTransitionOut(nextState, StateLeavingConditions.LeavingState);
        yield return oldState?.ExitState(nextState, StateLeavingConditions.LeavingState);

        yield return WarmUpAndStartCurrentState(oldState);
    }

    /// <summary>
    /// Prepares and starts the <see cref="CurrentState"/>, if there is one.
    /// </summary>
    /// <param name="lastState">The previously active state.</param>
    private IEnumerator WarmUpAndStartCurrentState(IGameplayState lastState)
    {
        if (CurrentState == null)
        {
            yield break;
        }

        NextState immediateNextState = CurrentState.ImmediateNextState(lastState);

        if (immediateNextState != null)
        {
            if (immediateNextState.LeavingConditions == StateLeavingConditions.PushNewState)
            {
                yield return PushNewState(immediateNextState.NextPushedState);
                yield break;
            }
            else
            {
                yield return EndCurrentState();
                yield break;
            }
        }

        CurrentState.SetUXProvider(UXProvider);
        yield return CurrentState.Load();
        yield return CurrentState.AnimateTransitionIn(lastState);
        CurrentState.SetControls(lastActiveControls);
        CurrentState.StartState(this, lastState);
        InTransitionState = false;
    }

    /// <summary>
    /// Ends every state currently in the machine.
    /// </summary>
    public IEnumerator CollapseAllStates()
    {
        InTransitionState = true;
        while (CurrentState != null)
        {
            yield return CurrentState.ExitState(null, StateLeavingConditions.CollapsingState);
            PresentStates.Pop();
        }
        InTransitionState = false;
    }

    public IEnumerator YieldUntilNotInTransitionalState()
    {
        while (InTransitionState)
        {
            yield return null;
        }
    }
}
