using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CombatClockTests
{
    GlobalStateMachine stateMachine;
    LabyrinthState labyrinthState;
    WarrencrawlInputs inputs;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        inputs = new WarrencrawlInputs();
        stateMachine = new GlobalStateMachine(inputs);
        labyrinthState = new LabyrinthState(new TestLabyrinthProvider());
        yield return SceneHelper.SetSceneHelper(stateMachine, inputs);
        yield return stateMachine.ChangeToState(labyrinthState);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return stateMachine.CollapseAllStates();
    }

    /// <summary>
    /// Asserts that taking a bunch of steps with the combat clock off does not start an encounter.
    /// </summary>
    [UnityTest]
    public IEnumerator ShouldEncounterStart_EncounterTriggers()
    {
        labyrinthState.ActiveCombatClock.Enable();

        int steps = 0;
        while (steps < labyrinthState.ActiveCombatClock.MaxStepsToEncounter)
        {
            yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Forward());
            steps++;

            if (stateMachine.CurrentState is BattleState)
            {
                break;
            }

            yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Backward());
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
        labyrinthState.ActiveCombatClock.Disable();

        int steps = 0;
        while (steps < labyrinthState.ActiveCombatClock.MaxStepsToEncounter)
        {
            yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Forward());
            steps++;

            if (stateMachine.CurrentState is BattleState)
            {
                break;
            }

            yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Backward());
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