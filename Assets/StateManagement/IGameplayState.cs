using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface describing the basic actions of a gameplay state, and what is needed to work with the <see cref="GlobalStateMachine"/>.
/// </summary>
public interface IGameplayState
{
    /// <summary>
    /// Unload and prepare a transition to the provided state.
    /// This preserves this state in the stack, for coming back to later.
    /// </summary>
    /// <param name="nextState"></param>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator TransitionUp();

    /// <summary>
    /// Unload and prepare a transition out of this state.
    /// This removes it from the stack, so it should be fully cleaned up.
    /// </summary>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator TransitionDown();

    /// <summary>
    /// Load the assets for this state. Does not display them yet.
    /// </summary>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator Load();

    /// <summary>
    /// Animate a transition in to this state. At this point the assets should all be loaded and ready for display.
    /// </summary>
    /// <returns>Yieldable IEnumerator.</returns>
    IEnumerator TransitionInto();
}
