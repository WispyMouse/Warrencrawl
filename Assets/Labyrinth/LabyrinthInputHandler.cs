using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WarrencrawlInputs;

/// <summary>
/// Class for handling inputs in the Labyrinth.
/// Sends commands back up to <see cref="LabyrinthState"/> when they should be processed.
/// </summary>
public class LabyrinthInputHandler : ILabyrinthActions
{
    bool animating { get; set; } = false;

    LabyrinthState referencedState { get; set; }

    public LabyrinthInputHandler(LabyrinthState forState)
    {
        referencedState = forState;
    }

    public void OnForward(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Step(Vector3Int.up));
    }

    public void OnBackward(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Step(Vector3Int.down));
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Step(Vector3Int.right));
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Step(Vector3Int.left));
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        // TODO: rotation!
    }

    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        // TODO: rotation!
    }
}
