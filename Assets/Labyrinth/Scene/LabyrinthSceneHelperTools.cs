using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthSceneHelperTools : SceneHelperTools
{
    public GameLevel CurrentLevel;

    /// <summary>
    /// The LayerMask for walkable surfaces, i.e. floor.
    /// </summary>
    public LayerMask Walkable;

    /// <summary>
    /// The LayerMask for blocked areas, i.e. wall.
    /// </summary>
    public LayerMask Blocked;

    public void ToTheTown()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.ChangeToState(new TownState()));
    }

    public void ToBattle()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.PushNewState(new BattleState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new LabyrinthState();
    }
}
