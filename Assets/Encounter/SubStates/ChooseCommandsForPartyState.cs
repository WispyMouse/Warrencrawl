using System.Collections;

namespace EncounterSubStates
{
    public class ChooseCommandsForPartyState : EncounterSubState
    {
        int CurPartyMemberIndex { get; set; } = 0;

        public ChooseCommandsForPartyState(EncounterState baseEncounterState) : base(baseEncounterState)
        {

        }

        public override IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            if (CurPartyMemberIndex >= BaseEncounterState.PlayerPartyPointer.PartyMembers.Count)
            {
                yield return stateMachine.EndCurrentState();
                yield break;
            }

            EncounterMember thisPartyMember = BaseEncounterState.PlayerPartyPointer.PartyMembers[CurPartyMemberIndex++];
            yield return stateMachine.PushNewState(new ChooseCommandForAllyState(BaseEncounterState, this, thisPartyMember));
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
