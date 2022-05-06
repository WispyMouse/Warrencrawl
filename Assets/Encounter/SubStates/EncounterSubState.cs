using System.Collections;

namespace EncounterSubStates
{
    public abstract class EncounterSubState : IGameplayState
    {
        protected EncounterState BaseEncounterState { get; private set; }

        public EncounterSubState(EncounterState baseEncounterState)
        {
            BaseEncounterState = baseEncounterState;
        }

        public virtual IEnumerator AnimateTransitionIn(IGameplayState previousState)
        {
            yield break;
        }

        public virtual IEnumerator AnimateTransitionOut(IGameplayState nextState, StateLeavingConditions leavingConditions)
        {
            yield break;
        }

        public virtual IEnumerator ExitState(IGameplayState nextState, StateLeavingConditions leavingConditions)
        {
            yield break;
        }

        public virtual IEnumerator Load()
        {
            yield break;
        }

        public virtual void SetControls(WarrencrawlInputs activeInput)
        {

        }

        public virtual IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
        {
            yield break;
        }

        public virtual void UnsetControls(WarrencrawlInputs activeInput)
        {

        }
    }
}
