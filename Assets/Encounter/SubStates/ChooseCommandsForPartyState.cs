using System.Collections;

namespace EncounterSubStates
{
    public class ChooseCommandsForPartyState : EncounterSubState
    {
        int CurPartyMemberIndex { get; set; } = 0;

        public ChooseCommandsForPartyState(EncounterState baseEncounterState) : base(baseEncounterState)
        {

        }

        public override NextState ImmediateNextState(IGameplayState previousState)
        {
            if (CurPartyMemberIndex >= BaseEncounterState.PlayerPartyPointer.PartyMembers.Count)
            {
                return NextState.EndCurrentState();
            }

            EncounterMember thisPartyMember = BaseEncounterState.PlayerPartyPointer.PartyMembers[CurPartyMemberIndex++];
            return NextState.PushNextState(new ChooseCommandForAllyState(BaseEncounterState, this, thisPartyMember));
        }

        public void UndoLastCommand()
        {
            if (BaseEncounterState.EncounterCommands.Count > 0)
            {
                BaseEncounterState.EncounterCommands.RemoveAt(BaseEncounterState.EncounterCommands.Count - 1);
            }
            
            CurPartyMemberIndex--;
        }
    }
}
