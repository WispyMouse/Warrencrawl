using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    /// This assumes no Interactives on top of it, or any situational modifiers.
    /// </summary>
    public bool DefaultWalkable;

    /// <summary>
    /// The worldspace Y that identifies the floor of this tile.
    /// </summary>
    public float Height;

    /// <summary>
    /// The color to show for this object when <see cref="LabyrinthSceneHelperToolsEditor.showCells"/> is checked.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public Color DebugColor
    {
        get
        {
            if (DefaultWalkable)
            {
                return new Color(.6f, 1f, 1f, .6f);
            }
            else
            {
                return new Color(1f, .4f, .4f, .6f);
            }
        }
    }

    /// <summary>
    /// The worldspace position of this tile. Returns the middle of the floor of the tile.
    /// </summary>
    public Vector3 Worldspace
    {
        get
        {
            return new Vector3(Coordinate.X, Height, Coordinate.Y);
        }
    }
}
