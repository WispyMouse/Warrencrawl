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
    /// Unload and prepare a transition to the provided state.
    /// This preserves this state in the stack, for coming back to later.
    /// </summary>
    /// <param name="nextState">State that is being transitioned in to. Should not be null.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator TransitionUp(IGameplayState nextState);

    /// <summary>
    /// Unload and prepare a transition out of this state.
    /// This removes it from the stack, so it should be fully cleaned up.
    /// </summary>
    /// <param name="nextState">State that is being transitioned in to, under this one. Can be null.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator ExitState(IGameplayState nextState);

    /// <summary>
    /// Load the assets for this state. Does not display them yet.
    /// </summary>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator Load();

    /// <summary>
    /// Animate a transition in to this state. At this point the assets should all be loaded and ready for display.
    /// </summary>
    /// <param name="previousState">The state before this one was loaded. Can be null.</param>
    /// <param name="nextState">State that is being transitioned in to.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState);

    /// <summary>
    /// Sets the top level controls that this state uses.
    /// Called whenever a new control method is found, or the state is entered.
    /// </summary>
    /// <param name="activeInput">The input map to update.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    void SetControls(WarrencrawlInputs activeInput);
}
