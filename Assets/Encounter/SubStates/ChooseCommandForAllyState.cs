using System.Collections;

namespace EncounterSubStates
{
    public class ChooseCommandForAllyState : EncounterSubState
    {
        EncounterMember ActingAlly { get; set; }

        EncounterCommand ChosenCommand { get; set; }
        ChooseCommandsForPartyState PartyState { get; set; }
        GlobalStateMachine StateMachine { get; set; }

        public ChooseCommandForAllyState(EncounterState baseEncounterState, ChooseCommandsForPartyState commandsForPartyState, EncounterMember thisMember) : base(baseEncounterState)
        {
            PartyState = commandsForPartyState;
            ActingAlly = thisMember;
        }

        public override void StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            StateMachine = stateMachine;
        }

        public void CommandChosen(string command)
        {
            ChosenCommand = new EncounterCommand(ActingAlly, command);
            StateMachine.StartPushNewState(new ChooseTargetState(BaseEncounterState, ChosenCommand));
        }

        public void BackedOut()
        {
            PartyState.UndoLastCommand();
            StateMachine.StartEndCurrentState();
        }

        public override NextState ImmediateNextState(IGameplayState previousState)
        {
            if (previousState is ChooseTargetState)
            {
                if (ChosenCommand == null || ChosenCommand.Target == null)
                {
                    // No target was set; we should return to the start and ask what action this party member is taking 
                }
                else
                {
                    // A target was chosen; add this command to the list and go back a level so the next party member can act
                    BaseEncounterState.EncounterCommands.Add(ChosenCommand);
                    return NextState.EndCurrentState();
                }
            }

            return null;
        }
    }
}