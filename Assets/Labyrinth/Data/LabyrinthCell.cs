using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure for one cell in the labyrinth.
/// This contains all of the information about its positioning and contents during design time.
/// </summary>
public class LabyrinthCell
{
    /// <summary>
    /// Coordinate position of the cell.
    /// You can find this cell if you look at (X, ?, Y).
    /// The y level in worldspace / z level in coordinate space does not have a one to one correlation.
    /// </summary>
    public CellCoordinates Coordinate { get; set; }

    /// <summary>
    /// Can this cell be walked on to?
    /// </summary>
    public bool Walkable { get; set; }

}
