using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxState : IGameplayState
{
    UIActions uiActions { get; set; }
    MessageBoxHandler messageBoxHandlerInstance { get; set; }
    string messageToShow { get; set; }

    public MessageBoxState(string message)
    {
        messageToShow = message;
    }

    public IEnumerator AnimateTransitionIn(IGameplayState previousState)
    {
        yield break;
    }

    public IEnumerator AnimateTransitionOut(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator ChangeUp(IGameplayState nextState)
    {
        messageBoxHandlerInstance.Close();
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        messageBoxHandlerInstance.Close();
        yield break;
    }

    public IEnumerator Load()
    {
        messageBoxHandlerInstance = GameObject.FindObjectOfType<MessageBoxHandler>();
        uiActions = GameObject.FindObjectOfType<UIActions>();
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {
        activeInput.UI.Enable();
        activeInput.UI.SetCallbacks(uiActions);
    }
    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        activeInput.UI.SetCallbacks(null);
        activeInput.UI.Disable();
    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        messageBoxHandlerInstance.ShowMessage(messageToShow, () => { Progress(stateMachine); });

        yield break;
    }

    void Progress(GlobalStateMachine stateMachine)
    {
        messageBoxHandlerInstance.StartCoroutine(stateMachine.EndCurrentState());
    }
}
