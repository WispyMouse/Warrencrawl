using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player's point of view in to a labyrinth.
/// </summary>
public class PointOfView : MonoBehaviour
{
    /// <summary>
    /// The direction the player is currently facing.
    /// </summary>
    public Direction CurFacing { get; set; }

    /// <summary>
    /// The position on the labyrinth map of the player.
    /// </summary>
    public CellCoordinates CurCoordinates { get; set; }
}
