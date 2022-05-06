using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EncounterMembersTests
{
    GlobalStateMachine stateMachine;
    WarrencrawlInputs inputs;

    [SetUp]
    public void SetUp()
    {
        inputs = new WarrencrawlInputs();
        stateMachine = new GlobalStateMachine(inputs, new ImmediateCoroutineRunner());
    }

    [UnityTearDown]
    public IEnumerator UnityTearDown()
    {
        yield return stateMachine.CollapseAllStates();
    }

    /// <summary>
    /// Checks that battles started with BattleOpponents 
    /// </summary>
    [UnityTest]
    public IEnumerator EncounterState_OpponentMembersAreAsExpected()
    {
        BattleOpponents expectedOpponents = new BattleOpponents();

        EncounterMember expectedOpponentA = new EncounterMember() { DisplayName = "Enemy A" };
        EncounterMember expectedOpponentB = new EncounterMember() { DisplayName = "Enemy B" };
        EncounterMember expectedOpponentC = new EncounterMember() { DisplayName = "Enemy C" };
        EncounterMember notExpectedOpponentD = new EncounterMember() { DisplayName = "Enemy D" };

        PlayerParty playerParty = new PlayerParty();
        playerParty.AddPartyMember(new EncounterMember() { DisplayName = "Party Member A" });

        expectedOpponents.AddOpposingMember(expectedOpponentA);
        expectedOpponents.AddOpposingMember(expectedOpponentB);
        expectedOpponents.AddOpposingMember(expectedOpponentC);

        EncounterState newState = new EncounterState(playerParty, expectedOpponents);

        yield return stateMachine.ChangeToState(newState);

        Assert.That(newState.Opponents.OpposingMembers, Does.Contain(expectedOpponentA));
        Assert.That(newState.Opponents.OpposingMembers, Does.Contain(expectedOpponentB));
        Assert.That(newState.Opponents.OpposingMembers, Does.Contain(expectedOpponentC));
        Assert.That(newState.Opponents.OpposingMembers, Does.Not.Contains(notExpectedOpponentD));

        Assert.That(newState.Opponents.OpposingMembers.Count, Is.EqualTo(3));
    }

    /// <summary>
    /// Checks that battles started with BattleOpponents 
    /// </summary>
    [UnityTest]
    public IEnumerator EncounterState_PartyMembersAreAsExpected()
    {
        BattleOpponents expectedOpponents = new BattleOpponents();

        EncounterMember partyMemberA = new EncounterMember() { DisplayName = "Party Member A" };
        EncounterMember partyMemberB = new EncounterMember() { DisplayName = "Party Member B" };
        EncounterMember partyMemberC = new EncounterMember() { DisplayName = "Party Member C" };
        EncounterMember inactivePartyMemberD = new EncounterMember() { DisplayName = "Inative Party Member D" };

        PlayerParty playerParty = new PlayerParty();
        playerParty.AddPartyMember(partyMemberA);
        playerParty.AddPartyMember(partyMemberB);
        playerParty.AddPartyMember(partyMemberC);

        expectedOpponents.AddOpposingMember(new EncounterMember() { DisplayName = "Enemy A" });

        EncounterState newState = new EncounterState(playerParty, expectedOpponents);

        yield return stateMachine.ChangeToState(newState);

        Assert.That(newState.PlayerPartyPointer.PartyMembers, Does.Contain(partyMemberA));
        Assert.That(newState.PlayerPartyPointer.PartyMembers, Does.Contain(partyMemberB));
        Assert.That(newState.PlayerPartyPointer.PartyMembers, Does.Contain(partyMemberC));
        Assert.That(newState.PlayerPartyPointer.PartyMembers, Does.Not.Contains(inactivePartyMemberD));

        Assert.That(newState.PlayerPartyPointer.PartyMembers.Count, Is.EqualTo(3));
    }
}
