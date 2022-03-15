using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the state of conflict between the player's party and another, usually of monsters of somesort.
/// </summary>
public class BattleState : SceneLoadingGameplayState
{
    public override string SceneName => "Battle";

    /// <summary>
    /// A reference to the current party. There should be one of these during gameplay, and when most of this script runs.
    /// </summary>
    public PlayerParty PlayerPartyPointer { get; set; }

    /// <summary>
    /// A reference to an opposing party for this conflict. Passed in the constructor or built during it.
    /// </summary>
    public BattleOpponents Opponents { get; set; }

    /// <summary>
    /// A list of commands to execute gameplay with.
    /// Set by the level above it, <see cref="ChooseCommandsForPartyState"/>, to be executed in <see cref="ResolveCommandsState"/>.
    /// </summary>
    public List<BattleCommand> BattleCommands { get; set; } = new List<BattleCommand>();

    public override void SetControls(WarrencrawlInputs controls)
    {
        // TODO: Enable battle controls
    }

    public BattleState()
    {
        Opponents = new BattleOpponents();
    }

    public BattleState(BattleOpponents opponents)
    {
        Opponents = opponents;
    }

    public override IEnumerator Load()
    {
        yield return base.Load();

        PlayerPartyPointer = SceneHelperInstance.PlayerParty;
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        if (previousState is ChooseCommandsForPartyState)
        {
            // If the previousState was a ChoosecommandsForPartyState, then we've chosen all of our commands; combine the player commands and the opponent commands and process.
            List<BattleCommand> allCommands = BattleCommands.Union(new List<BattleCommand>() 
            {
                new BattleCommand(), new BattleCommand(), new BattleCommand(), new BattleCommand(), new BattleCommand(), new BattleCommand() 
            }).ToList();

            BattleCommands.Clear();

            yield return globalStateMachine.PushNewState(new ResolveCommandsState(this, allCommands));
        }
        else
        {
            yield return globalStateMachine.PushNewState(new ChooseCommandsForPartyState(this, globalStateMachine));
        }
    }
}
