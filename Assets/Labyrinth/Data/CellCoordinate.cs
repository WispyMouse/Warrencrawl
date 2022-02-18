using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure for a specific point in space.
/// CellCoordinates have a zero-to-one relationship with <see cref="LabyrinthCell"/>. A theoretical CellCoordinate that doesn't exist
/// in a <see cref="LabyrinthLevel"/>, and would map to null.
/// </summary>
public struct CellCoordinates : IEquatable<CellCoordinates>
{
    /// <summary>
    /// x coordinate.
    /// This represents left and right in both the 3d space and on paper.
    /// You can reliably find something in world space by looking for its X position.
    /// </summary>
    public int X;

    /// <summary>
    /// y coordinate.
    /// This represents forward and backward in the 3d space, or upward and downward on paper.
    /// You can reliably find something in world space by looking for its Y position, as the Z position of it.
    /// </summary>
    public int Y;

    /// <summary>
    /// z coordinate.
    /// This represents up and down in the 3d space, or closer or farther away on paper. (It'd probably have to be on another page or on its own thing)
    /// Unlike X and Y, the Z level doesn't map reliably to space, as making Z level using maps in a tile based game requires some magic.
    /// </summary>
    public int Z;

    public CellCoordinates(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>
    /// Returns a collection of the coordinates that are orthogonal neighbors to this one; north, east, south, west.
    /// These are not always the only possible neighbors; going up a slope will often increase your z level, which isn't technically part of this neighbor list.
    /// </summary>
    public IEnumerable<CellCoordinates> OrthogonalNeighbors
    {
        get
        {
            return new List<CellCoordinates>()
            {
                new CellCoordinates(X + 1, Y, Z),
                new CellCoordinates(X, Y + 1, Z),
                new CellCoordinates(X - 1, Y, Z),
                new CellCoordinates(X, Y - 1, Z)
            };
        }
    }

    /// <summary>
    /// Returns a CellCoordinates for 0, 0, 0.
    /// </summary>
    public static CellCoordinates Zero
    {
        get
        {
            return new CellCoordinates(0, 0, 0);
        }
    }

    public bool Equals(CellCoordinates other)
    {
        return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
    }

    public static bool operator ==(CellCoordinates a, CellCoordinates b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(CellCoordinates a, CellCoordinates b)
    {
        return !(a == b);
    }
}
