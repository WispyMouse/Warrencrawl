using System.Collections;

namespace EncounterSubStates
{
    public class ChooseTargetState : EncounterSubState
    {
        EncounterCommand Command { get; set; }

        public ChooseTargetState(EncounterState baseEncounterState, EncounterCommand command) : base (baseEncounterState)
        {
            Command = command;
        }

        public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            while (Command != null && Command.Target == null)
            {
                yield return null;
            }

            yield return stateMachine.EndCurrentState();
        }

        public void TargetChosen(EncounterMember target)
        {
            Command.SetTarget(target);
        }

        public void BackedOut()
        {
            Command = null;
        }
    }
}