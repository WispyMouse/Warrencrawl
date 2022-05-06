/// <summary>
/// Describes what type of state transition is occurring.
/// </summary>
public enum StateLeavingConditions
{
    /// <summary>
    /// There is a new state that will be pushed on top of this one. This state should not leave memory entirely.
    /// </summary>
    PushNewState,

    /// <summary>
    /// This state is being left so that a new state can be chagned to. This state should be cleaned up and leave memory.
    /// </summary>
    LeavingState,

    /// <summary>
    /// This state is being collapsed as part of the State Machine collapsing all states, and should expressly remove from memory.
    /// </summary>
    CollapsingState
}