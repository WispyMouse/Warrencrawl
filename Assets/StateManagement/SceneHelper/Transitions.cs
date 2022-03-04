using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    public TransitionInputAllowanceState InputAllowance = TransitionInputAllowanceState.NotInTransition;
    public bool AnimationFinished;

    public Animator AnimatorInstance;

    public IEnumerator TransitionIn()
    {
        AnimationFinished = false;

        AnimatorInstance.SetTrigger("TransitionIn");

        yield return new WaitUntil(() => AnimationFinished);
    }

    public IEnumerator TransitionOut()
    {
        AnimationFinished = false;

        AnimatorInstance.SetTrigger("TransitionOut");

        yield return new WaitUntil(() => AnimationFinished);
    }

    public void SetInputAllowance(TransitionInputAllowanceState stage)
    {
        InputAllowance = stage;
    }

    public void Finished()
    {
        AnimationFinished = true;
        InputAllowance = TransitionInputAllowanceState.NotInTransition;
    }

    public IEnumerator ContinueTransitionYieldUntilInputsOK()
    {
        AnimationFinished = false;

        AnimatorInstance.SetTrigger("TransitionOut");

        yield return new WaitUntil(() => AnimationFinished || InputAllowance == TransitionInputAllowanceState.InputsOK);
    }
}
