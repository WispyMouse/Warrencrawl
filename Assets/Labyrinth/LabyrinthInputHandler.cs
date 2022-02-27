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

        referencedState.Animator.StartCoroutine(referencedState.Step(referencedState.PointOfViewInstance.CurFacing.Forward()));
    }

    public void OnBackward(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Step(referencedState.PointOfViewInstance.CurFacing.Backward()));
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Step(referencedState.PointOfViewInstance.CurFacing.Right()));
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Step(referencedState.PointOfViewInstance.CurFacing.Left()));
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Rotate(referencedState.PointOfViewInstance.CurFacing.RotateRight()));
    }

    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        if (animating)
        {
            return;
        }

        referencedState.Animator.StartCoroutine(referencedState.Rotate(referencedState.PointOfViewInstance.CurFacing.RotateLeft()));
    }
}
