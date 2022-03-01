using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LabyrinthTraversalTests
{
    /// <summary>
    /// Asserts that you can take a step forward in the labyrinth, and that it'll change your position.
    /// </summary>
    [UnityTest]
    public IEnumerator Step_CanStepForwardWhenIntended()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine(new WarrencrawlInputs());

        LabyrinthState state = new LabyrinthState(new TestLabyrinthProvider());

        yield return stateMachine.ChangeToState(state);

        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));

        yield return state.Step(state.PointOfViewInstance.CurFacing.Forward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        // should fail to step forward because of the wall at 0, 2, 0
        yield return state.Step(state.PointOfViewInstance.CurFacing.Forward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));
    }

    /// <summary>
    /// Asserts that turning to the side and trying to move results in predictable movement.
    /// </summary>
    [UnityTest]
    public IEnumerator Step_CanStrafeWhenIntended()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine(new WarrencrawlInputs());

        LabyrinthState state = new LabyrinthState(new TestLabyrinthProvider());

        yield return stateMachine.ChangeToState(state);

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.East));

        // Should fail to move forward because no tile in that direction
        yield return state.Step(state.PointOfViewInstance.CurFacing.Forward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));

        yield return state.Step(state.PointOfViewInstance.CurFacing.Left());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        // Should fail to move to the left because of the tile not being walkable
        yield return state.Step(state.PointOfViewInstance.CurFacing.Left());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateLeft());
        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.West));

        yield return state.Step(state.PointOfViewInstance.CurFacing.Left());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));
    }

    /// <summary>
    /// Validates that the player can rotate all the way to the right and all the way to the left, and that the directions being faced are as expected.
    /// </summary>
    [UnityTest]
    public IEnumerator Rotate_CanRotateAround()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine(new WarrencrawlInputs());

        LabyrinthState state = new LabyrinthState(new TestLabyrinthProvider());

        yield return stateMachine.ChangeToState(state);

        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.North));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.East));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.South));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.West));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateRight());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.North));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.West));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.South));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.East));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateLeft());
        Assert.That(state.PointOfViewInstance.CurFacing, Is.EqualTo(Direction.North));
    }

    /// <summary>
    /// Asserts that you can take a step backward when intended.
    /// </summary>
    [UnityTest]
    public IEnumerator Step_CanStepBackward()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine(new WarrencrawlInputs());

        LabyrinthState state = new LabyrinthState(new TestLabyrinthProvider());

        yield return stateMachine.ChangeToState(state);

        yield return state.Step(state.PointOfViewInstance.CurFacing.Backward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, -1, 0)));

        // Should fail to move in to non-walkable space
        yield return state.Step(state.PointOfViewInstance.CurFacing.Backward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, -1, 0)));

        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateRight());
        yield return state.Rotate(state.PointOfViewInstance.CurFacing.RotateRight());

        yield return state.Step(state.PointOfViewInstance.CurFacing.Backward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 0, 0)));

        yield return state.Step(state.PointOfViewInstance.CurFacing.Backward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));

        // Should fail to move in to non-walkable space
        yield return state.Step(state.PointOfViewInstance.CurFacing.Backward());
        Assert.That(state.PointOfViewInstance.CurCoordinates, Is.EqualTo(new CellCoordinates(0, 1, 0)));
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
            return testLevel;
        }
    }
}