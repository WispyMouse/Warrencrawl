using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General handler for the graphical elements of animations.
/// Doesn't update the object's position in the world (such as <see cref="PointOfView.CurCoordinates"/>), just the animated movement.
/// Things should yield the appropriate animation and then set the object's position.
/// </summary>
public class LabyrinthAnimationHandler : MonoBehaviour
{
    /// <summary>
    /// Runs an animation for stepping in the specified direction.
    /// The movement is treated as one step, regardless of distance.
    /// </summary>
    /// <param name="pov">The object to move.</param>
    /// <param name="toWalkTo">The cell to walk to.</param>
    public IEnumerator StepTo(PointOfView pov, LabyrinthCell toWalkTo)
    {
        Vector3 startingPosition = pov.transform.position;
        Vector3 targetPosition = toWalkTo.Worldspace;
        float curTime = 0;
        float stepTime = .3f;

        while (curTime < stepTime)
        {
            if (pov.gameObject == null || !pov.gameObject.activeSelf)
            {
                break;
            }

            curTime += Time.deltaTime;
            pov.transform.position = Vector3.Lerp(startingPosition, targetPosition, curTime / stepTime);
            yield return new WaitForEndOfFrame();
        }

        if (pov.gameObject == null || !pov.gameObject.activeSelf)
        {
            yield break;
        }

        pov.transform.position = targetPosition;
    }

    /// <summary>
    /// Runs an animation for rotating towards a specified direction.
    /// </summary>
    /// <param name="pov">The object to move.</param>
    /// <param name="newFacing">The direction to face.</param>
    public IEnumerator Rotate(PointOfView pov, Direction newFacing)
    {
        Vector3 startingFacing = pov.transform.rotation.eulerAngles;
        Vector3 targetFacing = new Vector3(0, newFacing.Degrees(), 0);
        float curTime = 0;
        float turnTime = .4f;

        while (curTime < turnTime)
        {
            pov.transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(startingFacing.y, targetFacing.y, curTime / turnTime), 0);
            curTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        pov.transform.rotation = Quaternion.Euler(targetFacing);
    }
}
