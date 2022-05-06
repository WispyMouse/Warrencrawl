public class NextState
{
    public IGameplayState NextPushedState { get; private set; }
    public StateLeavingConditions LeavingConditions { get; private set; }

    private NextState()
    {

    }

    public static NextState EndCurrentState()
    {
        NextState toNextState = new NextState();
        toNextState.LeavingConditions = StateLeavingConditions.LeavingState;
        return toNextState;
    }

    public static NextState PushNextState(IGameplayState toPush)
    {
        NextState toNextState = new NextState();
        toNextState.NextPushedState = toPush;
        toNextState.LeavingConditions = StateLeavingConditions.PushNewState;
        return toNextState;
    }
}