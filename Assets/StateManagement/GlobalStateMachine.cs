using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the <see cref="IGameplayState"/>s of the game.
/// If you want to transition from one state to another, that goes through here.
/// Receives input and passes it to the current active state.
/// </summary>
public class GlobalStateMachine
{
    /// <summary>
    /// The stack of states currently loaded in to memory.
    /// States underneath the top may be "inactive" and out of memory, and the top state may not be fully "active".
    /// </summary>
    Stack<IGameplayState> PresentStates { get; set; } = new Stack<IGameplayState>();

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
        IGameplayState oldState = CurrentState;
        if (oldState != null)
        {
            yield return oldState.TransitionDown(newState);
            PresentStates.Pop();
        }

        PresentStates.Push(newState);
        yield return newState.Load();
        yield return newState.TransitionInto(oldState);
    }

    /// <summary>
    /// Transitions to a new state, finishing when complete.
    /// The old state remains, lower on the <see cref="PresentStates"/> stack.
    /// </summary>
    /// <param name="newState">The state to transition in to.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator PushNewState(IGameplayState newState)
    {
        IGameplayState oldState = CurrentState;
        yield return oldState?.TransitionUp(newState);
        PresentStates.Push(newState);
        yield return newState.Load();
        yield return newState.TransitionInto(oldState);
    }

    /// <summary>
    /// Transitions out of the current state to the one below it.
    /// </summary>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator EndCurrentState()
    {
        IGameplayState oldState = CurrentState;
        PresentStates.Pop();

        PresentStates.TryPeek(out IGameplayState nextState);
        yield return oldState?.TransitionDown(nextState);

        yield return nextState?.Load();
        yield return nextState?.TransitionInto(oldState);
    }
}
