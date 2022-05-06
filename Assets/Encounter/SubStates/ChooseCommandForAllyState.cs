using System.Collections;

namespace EncounterSubStates
{
    public class ChooseCommandForAllyState : EncounterSubState
    {
        EncounterMember ActingAlly { get; set; }
        bool BackingOut { get; set; } = false;

        EncounterCommand ChosenCommand { get; set; }
        ChooseCommandsForPartyState PartyState { get; set; }

        public ChooseCommandForAllyState(EncounterState baseEncounterState, ChooseCommandsForPartyState commandsForPartyState, EncounterMember thisMember) : base(baseEncounterState)
        {
            PartyState = commandsForPartyState;
            ActingAlly = thisMember;
        }

        public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
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
                    yield return stateMachine.EndCurrentState();
                    yield break;
                }
            }

            while (ChosenCommand == null && BackingOut == false)
            {
                yield return null;
            }

            if (BackingOut)
            {
                PartyState.UndoLastCommand();
                yield return stateMachine.EndCurrentState();
                yield break;
            }

            yield return stateMachine.PushNewState(new ChooseTargetState(BaseEncounterState, ChosenCommand));
        }

        public void CommandChosen(string command)
        {
            ChosenCommand = new EncounterCommand(ActingAlly, command);
        }

        public void BackedOut()
        {
            BackingOut = true;
        }
    }
}