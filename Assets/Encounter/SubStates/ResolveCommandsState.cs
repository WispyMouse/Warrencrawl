using System.Collections;
using System.Collections.Generic;

namespace EncounterSubStates
{
    public class ResolveCommandsState : EncounterSubState
    {
        List<EncounterCommand> CommandsToResolve { get; set; }

        public ResolveCommandsState(EncounterState baseEncounterState, List<EncounterCommand> commands) : base(baseEncounterState)
        {
            CommandsToResolve = commands;
        }

        public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            if (CommandsToResolve.Count == 0)
            {
                yield return stateMachine.EndCurrentState();
                yield break;
            }

            EncounterCommand nextCommand = CommandsToResolve[0];
            CommandsToResolve.RemoveAt(0);

            yield return stateMachine.PushNewState(new ResolveCommandState(BaseEncounterState, nextCommand));
        }
    }
}
