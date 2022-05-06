using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static WarrencrawlInputs;

/// <summary>
/// Class for handling inputs in the Labyrinth.
/// Sends commands back up to <see cref="LabyrinthState"/> when they should be processed.
/// </summary>
public class LabyrinthInputHandler : MonoBehaviour, ILabyrinthActions
{
    SceneHelper sceneHelperInstance { get; set; }

    /// <summary>
    /// If there is a locking animation being played currently.
    /// </summary>
    bool animating { get; set; } = false;

    /// <summary>
    /// A reference to the active LabyrinthState that this is handling inputs for.
    /// Set by <see cref="Initialize(LabyrinthState)"/>.
    /// </summary>
    LabyrinthState referencedState { get; set; }

    /// <summary>
    /// Whether this input handler has been initialized yet.
    /// Set to true by <see cref="Initialize(LabyrinthState)"/>.
    /// </summary>
    bool initialized { get; set; } = false;

    /// <summary>
    /// The buffered inputs for the player.
    /// Each input is stacked on top of eachother, with the top most one being evaluated in <see cref="Update"/>.
    /// These are present until the relevant input is not being held down.
    /// </summary>
    InputStack bufferedActions { get; set; } = new InputStack();

    /// <summary>
    /// Initializes the Input Handler with everything it needs to run.
    /// </summary>
    /// <param name="forState">The LabyrinthState to handle inputs for.</param>
    public void Initialize(LabyrinthState forState)
    {
        referencedState = forState;

        forState.LockingAnimationFinished += AnimationFinished;

        sceneHelperInstance = GameObject.FindObjectOfType<SceneHelper>();

        initialized = true;
    }

    void Update()
    {
        if (!CanAnimate())
        {
            return;
        }

        if (bufferedActions.CurrentInput == null)
        {
            return;
        }

        animating = true;
        BufferedInput bufferedInput = bufferedActions.CurrentInput;
        sceneHelperInstance.PlayCoroutine(bufferedInput.FunctionToPerform());
    }

    public void OnForward(InputAction.CallbackContext context)
    {
        MoveDirection(context, RelativeDirection.Forward);
    }

    public void OnBackward(InputAction.CallbackContext context)
    {
        MoveDirection(context, RelativeDirection.Backward);
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        MoveDirection(context, RelativeDirection.Right);
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        MoveDirection(context, RelativeDirection.Left);
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        ApplyCoroutineContinuously(context, () => referencedState.Rotate(referencedState.PointOfViewInstance.CurFacing.RotateRight()));
    }

    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        ApplyCoroutineContinuously(context, () => referencedState.Rotate(referencedState.PointOfViewInstance.CurFacing.RotateLeft()));
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (!CanAnimate())
        {
            return;
        }

        animating = true;
        sceneHelperInstance.PlayCoroutine(referencedState.Interact());
    }

    void MoveDirection(InputAction.CallbackContext context, RelativeDirection relativeMovement)
    {
        ApplyCoroutineContinuously(context, () => referencedState.Step(referencedState.PointOfViewInstance.CurFacing.EvaluateRelativeDirection(relativeMovement)));
    }

    /// <summary>
    /// Sets a function that returns an IEnumerator to apply whenever there isn't another locking animation playing.
    /// </summary>
    /// <param name="context">Context for the input being processed. Used to determine if the input should be ignored, like if the input is being released.</param>
    /// <param name="toApply">The IEnumerator to apply whenever able.</param>
    void ApplyCoroutineContinuously(InputAction.CallbackContext context, Func<IEnumerator> toApply)
    {
        if (context.canceled)
        {
            bufferedActions.Remove(context.action);
            return;
        }

        bufferedActions.Add(new BufferedInput(context.action, toApply));
    }

    /// <summary>
    /// Callback for when a locking animation is finished playing.
    /// Subscribed to <see cref="LabyrinthState.LockingAnimationFinished"/>.
    /// </summary>
    /// <param name="o">The sender for the event.</param>
    /// <param name="e">Event arguments for the animation being finished. Currently unused.</param>
    void AnimationFinished(object o, EventArgs e)
    {
        animating = false;
    }

    /// <summary>
    /// Determines if input is ready to be processed.
    /// </summary>
    /// <returns>True if ready, false otherwise.</returns>
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
