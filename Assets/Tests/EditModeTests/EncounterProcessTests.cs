using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EncounterSubStates;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EncounterProcessTests
{
    GlobalStateMachine stateMachine;
    WarrencrawlInputs inputs;
    ProgrammaticUXProvider uxProvider;

    [SetUp]
    public void SetUp()
    {
        inputs = new WarrencrawlInputs();
        uxProvider = new ProgrammaticUXProvider();
        stateMachine = new GlobalStateMachine(inputs, uxProvider);
    }

    /// <summary>
    /// Checks that a combat can be started, each member can have their action selected, and that the resolve step leads back to the start.
    /// </summary>
    [UnityTest]
    public IEnumerator HappyPathFlowTest()
    {
        PlayerParty testParty = PlayerParty.GetDemoParty();
        BattleOpponents testOpponents = BattleOpponents.GetDemoOpponents();

        EncounterState testEncounter = new EncounterState(testParty, testOpponents);
        yield return stateMachine.ChangeToState(testEncounter);

        for (int ii = 0; ii < testParty.PartyMembers.Count; ii++)
        {
            Assert.That(stateMachine.CurrentState, Is.InstanceOf<ChooseCommandForAllyState>());
            ChooseCommandForAllyState allyState = stateMachine.CurrentState as ChooseCommandForAllyState;

            allyState.CommandChosen("Foo");
            yield return stateMachine.YieldUntilNotInTransitionalState();

            Assert.That(stateMachine.CurrentState, Is.InstanceOf<ChooseTargetState>());
            ChooseTargetState chooseTargetState = stateMachine.CurrentState as ChooseTargetState;

            chooseTargetState.TargetChosen(testOpponents.OpposingMembers[0]);
            yield return stateMachine.YieldUntilNotInTransitionalState();
        }

        // should then collapse to ChooseCommandsForPartyState, then EncounterState, then push to ResolveCommandsState, then push to ResolveCommand and go through each command
        // then back to EncounterState, which pushes a new ChooseCommandsForPartyState, finally going to another ChooseCommandsForAllyState
        // todo: how do we ensure that these motions are followed?
        Assert.That(stateMachine.CurrentState, Is.InstanceOf<ChooseCommandForAllyState>());
    }

    [UnityTearDown]
    public IEnumerator UnityTearDown()
    {
        yield return stateMachine.CollapseAllStates();
    }
}
