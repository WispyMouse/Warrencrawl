using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StateManagementTests
{
    /// <summary>
    /// Asserts that a Main Menu State can be loaded, then unloaded.
    /// </summary>
    [UnityTest]
    public IEnumerator StateLoadsAndUnloadsScenes()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine();

        Assert.That(stateMachine.CurrentState, Is.Null);

        MainMenuState newState = new MainMenuState();

        yield return stateMachine.TransitionIntoState(newState);
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
    public IEnumerator TypicalGameRoute()
    {
        GlobalStateMachine stateMachine = new GlobalStateMachine();

        MainMenuState mainMenuState = new MainMenuState();
        yield return stateMachine.TransitionOverToState(mainMenuState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(mainMenuState));

        TownState townState = new TownState();
        yield return stateMachine.TransitionOverToState(townState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(townState));

        LabyrinthState labyrinthState = new LabyrinthState();
        yield return stateMachine.TransitionOverToState(labyrinthState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(labyrinthState));

        BattleState battleState = new BattleState();
        yield return stateMachine.TransitionIntoState(battleState);
        Assert.That(stateMachine.CurrentState, Is.EqualTo(battleState));

        yield return stateMachine.EndCurrentState();
        Assert.That(stateMachine.CurrentState, Is.EqualTo(labyrinthState));
    }
}
