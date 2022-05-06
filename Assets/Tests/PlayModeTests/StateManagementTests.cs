using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StateManagementTests
{
    GlobalStateMachine stateMachine;
    WarrencrawlInputs inputs;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        inputs = new WarrencrawlInputs();
        stateMachine = new GlobalStateMachine(inputs);
        yield return SceneHelper.SetSceneHelper(stateMachine, inputs);
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
        MainMenuState menuState = new MainMenuState();

        yield return stateMachine.PushNewState(menuState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(menuState));

        // todo: Perhaps we want a function that fetches whether or not a scene is loaded
        Assert.That(StaticSceneTools.IsSceneLoaded(menuState.SceneName), Is.True);

        yield return stateMachine.EndCurrentState();
        Assert.That(stateMachine.CurrentState, Is.Null);
        Assert.That(StaticSceneTools.IsSceneLoaded(menuState.SceneName), Is.False);
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

        EncounterState encounterState = new EncounterState(PlayerParty.GetDemoParty(), BattleOpponents.GetDemoOpponents());
        yield return stateMachine.PushNewState(encounterState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(encounterState));

        yield return stateMachine.EndCurrentState();
        Assert.That(stateMachine.CurrentState, Is.EqualTo(labyrinthState));
    }
}
