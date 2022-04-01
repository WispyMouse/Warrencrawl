using UnityEngine;
using UnityEngine.InputSystem;
using static WarrencrawlInputs;

public class UIActions : MonoBehaviour, IUIActions
{
    MessageBoxHandler messageBoxHandler { get; set; }

    void Start()
    {
        messageBoxHandler = FindObjectOfType<MessageBoxHandler>();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Sending performed, messageboxhandler progresses");
            messageBoxHandler.Progress();
        }
    }
}