using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WarrencrawlInputs;

/// <summary>
/// Class for handling inputs in the Labyrinth.
/// Sends commands back up to <see cref="LabyrinthState"/> when they should be processed.
/// </summary>
public class LabyrinthInputHandler : MonoBehaviour, ILabyrinthActions
{
    bool animating { get; set; } = false;

    LabyrinthState referencedState { get; set; }

    bool initialized { get; set; } = false;

    Func<IEnumerator> bufferedAction { get; set; }

    public void Initialize(LabyrinthState forState)
    {
        referencedState = forState;

        forState.LockingAnimationFinished += AnimationFinished;

        initialized = true;
    }

    void Update()
    {
        if (!CanAnimate())
        {
            return;
        }

        if (bufferedAction == null)
        {
            return;
        }

        StartCoroutine(bufferedAction());
    }

    public void OnForward(InputAction.CallbackContext context)
    {
        MoveDirection(context, referencedState.PointOfViewInstance.CurFacing.Forward());
    }

    public void OnBackward(InputAction.CallbackContext context)
    {
        MoveDirection(context, referencedState.PointOfViewInstance.CurFacing.Backward());
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        MoveDirection(context, referencedState.PointOfViewInstance.CurFacing.Right());
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        MoveDirection(context, referencedState.PointOfViewInstance.CurFacing.Left());
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        ApplyCoroutineContinuously(context, () => referencedState.Rotate(referencedState.PointOfViewInstance.CurFacing.RotateRight()));
    }

    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        ApplyCoroutineContinuously(context, () => referencedState.Rotate(referencedState.PointOfViewInstance.CurFacing.RotateLeft()));
    }

    void MoveDirection(InputAction.CallbackContext context, Vector3Int toMove)
    {
        ApplyCoroutineContinuously(context, () => referencedState.Step(toMove));
    }

    void ApplyCoroutineContinuously(InputAction.CallbackContext context, Func<IEnumerator> toApply)
    {
        if (context.canceled)
        {
            bufferedAction = null;
            return;
        }

        bufferedAction = toApply;
    }

    void AnimationFinished(object o, EventArgs e)
    {
        animating = false;
    }

    bool CanAnimate()
    {
        if (!initialized)
        {
            return false;
        }

        if (animating)
        {
            return false;
        }

        return true;
    }
}
