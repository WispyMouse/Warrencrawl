using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handler for the transition <see cref="Animator"/>, providing yieldable coroutines.
/// </summary>
public class Transitions : MonoBehaviour
{
    public TransitionInputAllowanceState InputAllowance = TransitionInputAllowanceState.NotInTransition;
    public bool AnimationFinished;

    public Animator AnimatorInstance;

    /// <summary>
    /// Starts a transition.
    /// When this is finished, the system should be ready for <see cref="FinishTransition"/> to play when it is ready.
    /// </summary>
    public IEnumerator StartTransition()
    {
        AnimationFinished = false;

        AnimatorInstance.SetTrigger("StartTransition");

        yield return new WaitUntil(() => AnimationFinished);
    }

    /// <summary>
    /// Continues a transition to its finish.
    /// After this is finished, the transition should be fully completed and relevant pieces inactive.
    /// </summary>
    /// <returns></returns>
    public IEnumerator FinishTransition()
    {
        AnimationFinished = false;

        AnimatorInstance.SetTrigger("FinishTransition");

        yield return new WaitUntil(() => AnimationFinished);
    }

    /// <summary>
    /// Function for setting the current phase of the transition.
    /// Called by <see cref="AnimationEvent"/>s in the Animator.
    /// </summary>
    /// <param name="stage"></param>
    public void SetInputAllowance(TransitionInputAllowanceState stage)
    {
        InputAllowance = stage;
    }

    /// <summary>
    /// Function for setting the animation state to finished, and to reset waiting variables.
    /// </summary>
    public void Finished()
    {
        AnimationFinished = true;
        InputAllowance = TransitionInputAllowanceState.NotInTransition;
    }

    /// <summary>
    /// Runs <see cref="FinishTransition"/>, but returns when the <see cref="InputAllowance"/> is set to InputsOK.
    /// </summary>
    public IEnumerator FinishTransitionYieldUntilInputsOK()
    {
        AnimationFinished = false;

        AnimatorInstance.SetTrigger("FinishTransition");

        yield return new WaitUntil(() => AnimationFinished || InputAllowance == TransitionInputAllowanceState.InputsOK);
    }
}
