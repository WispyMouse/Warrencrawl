using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StairInteractiveTests
{
    GlobalStateMachine stateMachine;
    LabyrinthState labyrinthState;
    WarrencrawlInputs inputs;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        inputs = new WarrencrawlInputs();
        stateMachine = new GlobalStateMachine(inputs, new ProgrammaticUXProvider());
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
    /// Asserts that you can take the stairs to leave the labyrinth.
    /// </summary>
    [UnityTest]
    public IEnumerator Interact_CanTakeStairsToTown()
    {
        Assert.That(stateMachine.CurrentState, Is.EqualTo(labyrinthState));

        // Should take the stairs immediately in front of the POV
        yield return labyrinthState.Interact();

        Assert.That(stateMachine.CurrentState, Is.TypeOf(typeof(TownState)));
        Assert.That(StaticSceneTools.IsSceneLoaded("Town"), Is.True);
    }

    class TestLabyrinthProvider : IGameLevelProvider
    {
        public GameLevel GetLevel()
        {
            GameLevel testLevel = ScriptableObject.CreateInstance<GameLevel>();
            testLevel.LabyrinthData = new LabyrinthLevel();
            testLevel.LabyrinthData.Cells = new List<LabyrinthCell>()
            {
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 0, 0), DefaultWalkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 1, 0), DefaultWalkable = false },
            };
            testLevel.LabyrinthData.LabyrinthInteractives = new List<InteractiveData>()
            {
                new InteractiveData() { Kind = InteractiveKind.Stairs, OnCoordinates = new HashSet<CellCoordinates> { new CellCoordinates(0, 1, 0)}}
            };
            testLevel.CombatClockEnabled = false;
            return testLevel;
        }
    }
}