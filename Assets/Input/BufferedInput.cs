using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BufferedInput
{
    public InputAction MappedAction { get; set; }
    public Func<IEnumerator> FunctionToPerform { get; set; }

    public BufferedInput(InputAction mappedAction, Func<IEnumerator> functionToPerform)
    {
        MappedAction = mappedAction;
        FunctionToPerform = functionToPerform;
    }
}
