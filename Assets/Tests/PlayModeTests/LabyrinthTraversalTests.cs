using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LabyrinthTraversalTests
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
    /// Asserts that you can take a step forward in the labyrinth, and that it'll change your position.
    /// </summary>
    [UnityTest]
    public IEnumerator Step_CanStepForwardWhenIntended()
    {
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));

        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Forward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        // should fail to step forward because of the wall at 0, 2, 0
        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Forward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));
    }

    /// <summary>
    /// Asserts that turning to the side and trying to move results in predictable movement.
    /// </summary>
    [UnityTest]
    public IEnumerator Step_CanStrafeWhenIntended()
    {
        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.East));

        // Should fail to move forward because no tile in that direction
        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Forward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));

        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Left());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        // Should fail to move to the left because of the tile not being walkable
        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Left());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateLeft());
        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.West));

        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Left());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));
    }

    /// <summary>
    /// Validates that the player can rotate all the way to the right and all the way to the left, and that the directions being faced are as expected.
    /// </summary>
    [UnityTest]
    public IEnumerator Rotate_CanRotateAround()
    {
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.North));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.East));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.South));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.West));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.North));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.West));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.South));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.East));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(labyrinthState.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.North));
    }

    /// <summary>
    /// Asserts that you can take a step backward when intended.
    /// </summary>
    [UnityTest]
    public IEnumerator Step_CanStepBackward()
    {
        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Backward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, -1, 0)));

        // Should fail to move in to non-walkable space
        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Backward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, -1, 0)));

        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateRight());
        yield return labyrinthState.Rotate(labyrinthState.PointOfViewInstance.CurFacing.RotateRight());

        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Backward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));

        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Backward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        // Should fail to move in to non-walkable space
        yield return labyrinthState.Step(labyrinthState.PointOfViewInstance.CurFacing.Backward());
        Assert.That(labyrinthState.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));
    }

    class TestLabyrinthProvider : IGameLevelProvider
    {
        public GameLevel GetLevel()
        {
            GameLevel testLevel = ScriptableObject.CreateInstance<GameLevel>();
            testLevel.LabyrinthData = new LabyrinthLevel();
            testLevel.LabyrinthData.Cells = new List<LabyrinthCell>()
            {
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, -2, 0), DefaultWalkable = false },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, -1, 0), DefaultWalkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 0, 0), DefaultWalkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 1, 0), DefaultWalkable = true },
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 2, 0), DefaultWalkable = false },
            };
            testLevel.CombatClockEnabled = false;
            return testLevel;
        }
    }
}