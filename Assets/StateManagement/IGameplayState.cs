using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interface describing the basic actions of a gameplay state, and what is needed to work with the <see cref="GlobalStateMachine"/>.
/// </summary>
public interface IGameplayState
{
    /// <summary>
    /// Processes a change to a new state.
    /// This preserves this state in the stack, for coming back to later.
    /// </summary>
    /// <param name="nextState">State that is being transitioned in to. Should not be null.</param>
    IEnumerator ChangeUp(IGameplayState nextState);

    /// <summary>
    /// Plays any animations that are related to this state no longer being at the front.
    /// </summary>
    /// <param name="nextState">The state that will be active next.</param>
    IEnumerator AnimateTransitionOut(IGameplayState nextState);

    /// <summary>
    /// Process leaving the state, intending to unload it entirely.
    /// </summary>
    /// <param name="nextState">State that is being transitioned in to, under this one. Can be null.</param>
    IEnumerator ExitState(IGameplayState nextState);

    /// <summary>
    /// Load the assets for this state. Does not display them yet.
    /// </summary>
    IEnumerator Load();

    /// <summary>
    /// Plays any animations that are related to this state coming to the front.
    /// At the end of this IEnumerator, the game should be ready to play and receive input.
    /// Start a coroutine (not yield it) for animations that play while the transitioning is finishing.
    /// </summary>
    /// <param name="previousState">The state that was active before this.</param>
    IEnumerator AnimateTransitionIn(IGameplayState previousState);

    /// <summary>
    /// Animate a transition in to this state. At this point the assets should all be loaded and ready for display.
    /// </summary>
    /// <param name="previousState">The state before this one was loaded. Can be null.</param>
    /// <param name="nextState">State that is being transitioned in to.</param>
    IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState);

    /// <summary>
    /// Sets the top level controls that this state uses.
    /// Called whenever a new control method is found, or the state is entered.
    /// </summary>
    /// <param name="activeInput">The input map to update.</param>
    void SetControls(WarrencrawlInputs activeInput);

    /// <summary>
    /// Unsets all control bindings made during <see cref="SetControls(WarrencrawlInputs)"/>
    /// </summary>
    /// <param name="activeInput">The input map to update.</param>
    void UnsetControls(WarrencrawlInputs activeInput);
}
