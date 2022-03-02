using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFinished : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transitions transitionInstance = GameObject.FindObjectOfType<Transitions>();
        transitionInstance.TransitionFinished();
    }
}
