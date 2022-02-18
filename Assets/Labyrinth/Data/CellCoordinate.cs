using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellCoordinate
{
    public int X;
    public int Y;
    public int Z;

    public CellCoordinate(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public IEnumerable<CellCoordinate> Neighbors
    {
        get
        {
            return new List<CellCoordinate>()
            {
                new CellCoordinate(X + 1, Y, Z),
                new CellCoordinate(X, Y + 1, Z),
                new CellCoordinate(X - 1, Y, Z),
                new CellCoordinate(X, Y - 1, Z)
            };
        }
    }
}
