using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension methods for <see cref="Direction"/> to handle relative facing and movement.
/// </summary>
public static class DirectionExtensions
{
    /// <summary>
    /// Gets the relative positioning of forward for a given facing.
    /// </summary>
    /// <param name="facing">The direction to consider.</param>
    /// <returns>One relative step forward.</returns>
    public static Vector3Int Forward(this Direction facing)
    {
        switch (facing)
        {
            default:
            case Direction.North:
                return Vector3Int.up;
            case Direction.East:
                return Vector3Int.right;
            case Direction.South:
                return Vector3Int.down;
            case Direction.West:
                return Vector3Int.left;
        }
    }

    /// <summary>
    /// Gets the relative positioning of forward for a given facing.
    /// </summary>
    /// <param name="facing">The direction to consider.</param>
    /// <returns>One relative step to the right.</returns>
    public static Vector3Int Right(this Direction facing)
    {
        switch (facing)
        {
            default:
            case Direction.North:
                return Vector3Int.right;
            case Direction.East:
                return Vector3Int.down;
            case Direction.South:
                return Vector3Int.left;
            case Direction.West:
                return Vector3Int.up;
        }
    }

    /// <summary>
    /// Gets the relative positioning of forward for a given facing.
    /// </summary>
    /// <param name="facing">The direction to consider.</param>
    /// <returns>One relative step backward.</returns>
    public static Vector3Int Backward(this Direction facing)
    {
        switch (facing)
        {
            default:
            case Direction.North:
                return Vector3Int.down;
            case Direction.East:
                return Vector3Int.left;
            case Direction.South:
                return Vector3Int.up;
            case Direction.West:
                return Vector3Int.right;
        }
    }

    /// <summary>
    /// Gets the relative positioning of forward for a given facing.
    /// </summary>
    /// <param name="facing">The direction to consider.</param>
    /// <returns>One relative step to the left.</returns>
    public static Vector3Int Left(this Direction facing)
    {
        switch (facing)
        {
            default:
            case Direction.North:
                return Vector3Int.left;
            case Direction.East:
                return Vector3Int.up;
            case Direction.South:
                return Vector3Int.right;
            case Direction.West:
                return Vector3Int.down;
        }
    }

    /// <summary>
    /// Rotates a given facing.
    /// </summary>
    /// <param name="facing">The starting direction.</param>
    /// <returns>A rotated direction.</returns>
    public static Direction RotateRight(this Direction facing)
    {
        switch (facing)
        {
            default:
            case Direction.North:
                return Direction.East;
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            case Direction.West:
                return Direction.North;
        }
    }

    /// <summary>
    /// Rotates a given facing.
    /// </summary>
    /// <param name="facing">The starting direction.</param>
    /// <returns>A rotated direction.</returns>
    public static Direction RotateLeft(this Direction facing)
    {
        switch (facing)
        {
            default:
            case Direction.North:
                return Direction.West;
            case Direction.East:
                return Direction.North;
            case Direction.South:
                return Direction.East;
            case Direction.West:
                return Direction.South;
        }
    }

    /// <summary>
    /// Returns the 360-degree rotation for the facing.
    /// The camera's transform.rotation.y should have this value.
    /// </summary>
    /// <param name="facing">The facing to consider.</param>
    /// <returns>Degrees for facing.</returns>
    public static float Degrees(this Direction facing)
    {
        switch (facing)
        {
            default:
            case Direction.North:
                return 0;
            case Direction.East:
                return 90f;
            case Direction.South:
                return 180f;
            case Direction.West:
                return 270f;
        }
    }

    /// <summary>
    /// Given a direction you're facing, and a relative direction to move, return a step in the relative direction.
    /// </summary>
    /// <param name="facing">The cardinal direction being faced.</param>
    /// <param name="relativeDirection">The direction to step.</param>
    /// <returns>One step in the appropriate direction.</returns>
    public static Vector3Int EvaluateRelativeDirection(this Direction facing, RelativeDirection relativeDirection)
    {
        switch (relativeDirection)
        {
            default:
            case RelativeDirection.Forward:
                return facing.Forward();
            case RelativeDirection.Right:
                return facing.Right();
            case RelativeDirection.Backward:
                return facing.Backward();
            case RelativeDirection.Left:
                return facing.Left();
        }
    }
}
