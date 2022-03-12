using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleState : SceneLoadingGameplayState
{
    public override string SceneName => "Battle";

    PlayerParty PlayerPartyPointer { get; set; }
    public List<BattleCommand> BattleCommands { get; set; } = new List<BattleCommand>();

    public override void SetControls(WarrencrawlInputs controls)
    {
        // TODO: Enable battle controls
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
            yield return globalStateMachine.PushNewState(new ChooseCommandsForPartyState(this, globalStateMachine, PlayerPartyPointer));
        }
    }
}
