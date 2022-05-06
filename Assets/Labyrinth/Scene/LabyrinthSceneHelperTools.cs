using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthSceneHelperTools : SceneHelperTools, IGameLevelProvider
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
    /// The LayerMask for interactive elements, i.e. stairs.
    /// </summary>
    public LayerMask Interactive;

    /// <summary>
    /// A default GameLevel to load if nothing is passed in to LabyrinthState.
    /// </summary>
    public GameLevel DefaultLevel;

    /// <summary>
    /// A pointer to the active LabyrinthInputHandler. Needs to have <see cref="LabyrinthInputHandler.Initialize(LabyrinthState)"/> run on it.
    /// </summary>
    public LabyrinthInputHandler InputHandler;

    /// <summary>
    /// A pointer to the active AnimationHandler.
    /// </summary>
    public LabyrinthAnimationHandler AnimationHandler;

    public void ToTheTown()
    {
        SceneHelperInstance.PlayCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new TownState()));
    }

    public void ToBattle()
    {
        SceneHelperInstance.PlayCoroutine(SceneHelper.GlobalStateMachineInstance.PushNewState(new EncounterState(SceneHelperInstance.PlayerParty, BattleOpponents.GetDemoOpponents())));
    }

    protected override IEnumerator StartChild()
    {
        LabyrinthState curState = (SceneHelper.GlobalStateMachineInstance.CurrentState as LabyrinthState);

        if (curState == null)
        {
            Debug.LogWarning($"Expected {nameof(LabyrinthState)} to be the {nameof(SceneHelper.GlobalStateMachineInstance.CurrentState)}, but it was not.");
            yield break;
        }

        InputHandler.Initialize(curState);

        CurrentLevel = curState.LevelToLoad;
    }

    public override IGameplayState GetNewDemoState()
    {
        return new LabyrinthState(this);
    }

    public GameLevel GetLevel()
    {
        return DefaultLevel;
    }
}
