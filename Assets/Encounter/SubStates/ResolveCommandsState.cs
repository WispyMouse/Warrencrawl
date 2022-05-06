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

        public override NextState ImmediateNextState(IGameplayState previousState)
        {
            if (CommandsToResolve.Count == 0)
            {
                return NextState.EndCurrentState();
            }

            EncounterCommand nextCommand = CommandsToResolve[0];
            CommandsToResolve.RemoveAt(0);

            return NextState.PushNextState(new ResolveCommandState(BaseEncounterState, nextCommand));
        }

        public override void StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            
        }
    }
}
