using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : SceneLoadingGameplayState
{
    public override string SceneName => "Battle";

    public override void SetControls(WarrencrawlInputs controls)
    {
        // TODO: Enable battle controls
    }

    public override IEnumerator ExitState(IGameplayState nextState)
    {
        yield return SceneHelperInstance.TransitionsInstance.TransitionIn();

        yield return base.ExitState(nextState);
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        yield return base.StartState(globalStateMachine, previousState);

        SceneHelperInstance.StartCoroutine(SceneHelperInstance.TransitionsInstance.TransitionOut());
        yield return new WaitUntil(() =>
            SceneHelperInstance.TransitionsInstance.AnimationFinished
            || SceneHelperInstance.TransitionsInstance.InputAllowance == TransitionInputAllowanceState.InputsOK);
    }
}
