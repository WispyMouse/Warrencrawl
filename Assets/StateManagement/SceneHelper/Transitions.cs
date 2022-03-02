using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    bool Finished { get; set; } = false;

    public Animator AnimatorInstance;

    public IEnumerator TransitionIn()
    {
        Finished = false;

        AnimatorInstance.SetTrigger("TransitionIn");

        yield return new WaitUntil(() => Finished);
    }

    public IEnumerator TransitionOut()
    {
        Finished = false;

        AnimatorInstance.SetTrigger("TransitionOut");

        yield return new WaitUntil(() => Finished);
    }

    public void TransitionFinished()
    {
        Finished = true;
    }
}
