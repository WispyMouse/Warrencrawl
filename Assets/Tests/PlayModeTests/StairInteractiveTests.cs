using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StairInteractiveTests
{
    /// <summary>
    /// Asserts that you can take the stairs to leave the labyrinth.
    /// </summary>
    [UnityTest]
    public IEnumerator Interact_CanTakeStairsToTown()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine(new WarrencrawlInputs());

        LabyrinthState state = new LabyrinthState(new TestLabyrinthProvider());

        yield return stateMachine.ChangeToState(state);

        Assert.That(stateMachine.CurrentState, Is.EqualTo(state));

        // Should take the stairs immediately in front of the POV
        yield return state.Interact();

        Assert.That(stateMachine.CurrentState, Is.TypeOf(typeof(TownState)));
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
                new LabyrinthCell() { Coordinate = new CellCoordinates(0, 1, 0), Walkable = false, Interactive = new InteractiveData() { Kind = InteractiveKind.Stairs } },
            };
            return testLevel;
        }
    }
}