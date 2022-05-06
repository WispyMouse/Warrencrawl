using System.Collections;
using UnityEngine;

namespace EncounterSubStates
{
    public class ResolveCommandState : EncounterSubState
    {
        EncounterCommand CommandToResolve { get; set; }

        public ResolveCommandState(EncounterState baseEncounterState, EncounterCommand commandToResolve) : base(baseEncounterState)
        {
            CommandToResolve = commandToResolve;
        }

        public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            Debug.Log($"Resolving command: {CommandToResolve.ActingMember.DisplayName} uses {CommandToResolve.Command} on {CommandToResolve.Target.DisplayName}");
            yield return stateMachine.EndCurrentState();
        }
    }
}
