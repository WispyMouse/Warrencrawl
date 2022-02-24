using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure for one cell in the labyrinth.
/// This contains all of the information about its positioning and contents during design time.
/// </summary>
[System.Serializable]
public class LabyrinthCell
{
    /// <summary>
    /// Coordinate position of the cell.
    /// </summary>
    public CellCoordinates Coordinate;

    /// <summary>
    /// Can this cell be walked on to?
    /// </summary>
    public bool Walkable;

    /// <summary>
    /// What is the worldspace Y that identifies the floor of this tile?
    /// </summary>
    public float Height;

    public Color DebugColor
    {
        get
        {
            if (Walkable)
            {
                return new Color(.6f, 1f, 1f, .6f);
            }
            else
            {
                return new Color(1f, .4f, .4f, .6f);
            }
        }
    }

    public Vector3 Worldspace
    {
        get
        {
            return new Vector3(Coordinate.X, Height, Coordinate.Y);
        }
    }
}
