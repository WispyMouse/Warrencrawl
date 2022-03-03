using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


public class LabyrinthInputTests : InputTestFixture
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
    public IEnumerator UnityTearDown()
    {
        yield return stateMachine.CollapseAllStates();
    }

    [UnityTest]
    public IEnumerator OnForward_StepsWork()
    {
        Gamepad newGamepad = InputSystem.AddDevice<Gamepad>();

        Press(newGamepad.dpad.up);
        yield break;
    }

    class TestLabyrinthProvider : IGameLevelProvider
    {
        public GameLevel GetLevel()
        {
            GameLevel testLevel = ScriptableObject.CreateInstance<GameLevel>();
            testLevel.LabyrinthData = new LabyrinthLevel();
            testLevel.LabyrinthData.Cells = new List<LabyrinthCell>()
            {
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, -2, 0), Walkable = false },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, -1, 0), Walkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 0, 0), Walkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 1, 0), Walkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 2, 0), Walkable = false },
            };
            testLevel.CombatClockEnabled = false;
            return testLevel;
        }
    }
}
