using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EncounterSubStates;

public class EncounterState : IGameplayState
{
    /// <summary>
    /// A reference to the current party, passed in through the constructor.
    /// </summary>
    public PlayerParty PlayerPartyPointer { get; set; }

    /// <summary>
    /// A reference to an opposing party for this conflict, passed in through the constructor
    /// </summary>
    public BattleOpponents Opponents { get; set; }

    /// <summary>
    /// A list of commands to execute gameplay with.
    /// Set by the level above it, <see cref="ChooseCommandsForPartyState"/>, to be executed in <see cref="ResolveCommandsState"/>.
    /// </summary>
    public List<EncounterCommand> EncounterCommands { get; set; } = new List<EncounterCommand>();

    public EncounterState(PlayerParty playerPartyPointer, BattleOpponents opponents)
    {
        PlayerPartyPointer = playerPartyPointer;
        Opponents = opponents;
    }

    public IEnumerator AnimateTransitionIn(IGameplayState previousState)
    {
        yield break;
    }

    public IEnumerator AnimateTransitionOut(IGameplayState nextState, StateLeavingConditions leavingConditions)
    {
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState, StateLeavingConditions leavingConditions)
    {
        yield break;
    }

    public IEnumerator Load()
    {
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {
        
    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        if (EncounterCommands.Count == 0)
        {
            yield return stateMachine.PushNewState(new ChooseCommandsForPartyState(this));
        }
        else
        {
            List<EncounterCommand> encounterCommandsToResolve = new List<EncounterCommand>(EncounterCommands);
            EncounterCommands.Clear();

            // Generate some random moves for each opponent
            foreach (EncounterMember opponent in Opponents.OpposingMembers)
            {
                int randomTarget = Random.Range(0, PlayerPartyPointer.PartyMembers.Count);

                EncounterCommand opponentCommand = new EncounterCommand(opponent, "Hassle");
                opponentCommand.SetTarget(PlayerPartyPointer.PartyMembers[randomTarget]);
                encounterCommandsToResolve.Add(opponentCommand);
            }

            yield return stateMachine.PushNewState(new ResolveCommandsState(this, encounterCommandsToResolve));
        }
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }
}
