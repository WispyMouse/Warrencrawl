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

    /// <summary>
    /// A default GameLevel to load if nothing is passed in to LabyrinthState.
    /// </summary>
    public GameLevel DefaultLevel;

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
