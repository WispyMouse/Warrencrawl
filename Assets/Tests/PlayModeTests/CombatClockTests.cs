using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CombatClockTests
{
    /// <summary>
    /// Asserts that taking a bunch of steps with the combat clock off does not start an encounter.
    /// </summary>
    [UnityTest]
    public IEnumerator ShouldEncounterStart_EncounterTriggers()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine(new WarrencrawlInputs());
        LabyrinthState state = new LabyrinthState(new TestLabyrinthProvider());
        yield return stateMachine.ChangeToState(state);

        int steps = 0;
        while (steps < state.ActiveCombatClock.MaxStepsToEncounter)
        {
            yield return state.Step(state.PointOfViewInstance.CurFacing.Forward());
            steps++;

            if (stateMachine.CurrentState is BattleState)
            {
                break;
            }

            yield return state.Step(state.PointOfViewInstance.CurFacing.Backward());
            steps++;

            if (stateMachine.CurrentState is BattleState)
            {
                break;
            }
        }

        Assert.That(stateMachine.CurrentState, Is.TypeOf<BattleState>());
    }

    /// <summary>
    /// Asserts that taking a bunch of steps with the combat clock off does not start an encounter.
    /// </summary>
    [UnityTest]
    public IEnumerator Disable_CombatDoesNotStart()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine(new WarrencrawlInputs());
        LabyrinthState state = new LabyrinthState(new TestLabyrinthProvider());
        yield return stateMachine.ChangeToState(state);

        state.ActiveCombatClock.Disable();

        int steps = 0;
        while (steps < state.ActiveCombatClock.MaxStepsToEncounter)
        {
            yield return state.Step(state.PointOfViewInstance.CurFacing.Forward());
            steps++;

            if (stateMachine.CurrentState is BattleState)
            {
                break;
            }

            yield return state.Step(state.PointOfViewInstance.CurFacing.Backward());
            steps++;

            if (stateMachine.CurrentState is BattleState)
            {
                break;
            }
        }

        Assert.That(stateMachine.CurrentState, Is.TypeOf<LabyrinthState>());
    }

    class TestLabyrinthProvider : IGameLevelProvider
    {
        public GameLevel GetLevel()
        {
            GameLevel testLevel = ScriptableObject.CreateInstance<GameLevel>();
            testLevel.LabyrinthData = new LabyrinthLevel();
            testLevel.LabyrinthData.Cells = new List<LabyrinthCell>()
            {
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 0, 0), Walkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 1, 0), Walkable = true },
            };
            testLevel.CombatClockEnabled = true;
            return testLevel;
        }
    }
}