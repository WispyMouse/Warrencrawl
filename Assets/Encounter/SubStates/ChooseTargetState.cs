using System.Collections;

namespace EncounterSubStates
{
    public class ChooseTargetState : EncounterSubState
    {
        EncounterCommand Command { get; set; }
        GlobalStateMachine stateMachineInstance { get; set; }

        public ChooseTargetState(EncounterState baseEncounterState, EncounterCommand command) : base (baseEncounterState)
        {
            Command = command;
        }

        public override void StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            stateMachineInstance = stateMachine;
        }

        public void TargetChosen(EncounterMember target)
        {
            Command.SetTarget(target);
            stateMachineInstance.StartEndCurrentState();
        }

        public void BackedOut()
        {
            Command = null;
            stateMachineInstance.StartEndCurrentState();
        }
    }
}