using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StateManagementTests
{
    GlobalStateMachine stateMachine { get; set; }

    [SetUp]
    public void SetUp()
    {
        stateMachine = new GlobalStateMachine(new WarrencrawlInputs());
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return stateMachine.CollapseAllStates();
    }

    /// <summary>
    /// Asserts that a Main Menu State can be loaded, then unloaded.
    /// </summary>
    [UnityTest]
    public IEnumerator PushNewState_ScenesAdding_AddsAndEndsScenes()
    {
        MainMenuState newState = new MainMenuState();

        yield return stateMachine.PushNewState(newState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(newState));

        // todo: Perhaps we want a function that fetches whether or not a scene is loaded
        Assert.That(SceneManager.GetAllScenes().Any(scene => scene.name == newState.SceneName), Is.True);

        yield return stateMachine.EndCurrentState();
        Assert.That(stateMachine.CurrentState, Is.Null);
        Assert.That(SceneManager.GetAllScenes().Any(scene => scene.name == newState.SceneName), Is.False);
    }

    /// <summary>
    /// Asserts that a player can go from the main menu, to the town, to the labyrinth, in to a battle, and out of battle.
    /// This is a pretty simple walk of the most major game scene managing states.
    /// </summary>
    [UnityTest]
    public IEnumerator ChangeToState_HappyPath_ChangesState()
    {
        MainMenuState mainMenuState = new MainMenuState();
        yield return stateMachine.ChangeToState(mainMenuState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(mainMenuState));

        TownState townState = new TownState();
        yield return stateMachine.ChangeToState(townState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(townState));

        LabyrinthState labyrinthState = new LabyrinthState(new LabyrinthSceneHelperGrabber());
        yield return stateMachine.ChangeToState(labyrinthState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(labyrinthState));

        BattleState battleState = new BattleState();
        yield return stateMachine.PushNewState(battleState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(battleState));

        yield return stateMachine.EndCurrentState();
        Assert.That(stateMachine.CurrentState, Is.EqualTo(labyrinthState));
    }
}
