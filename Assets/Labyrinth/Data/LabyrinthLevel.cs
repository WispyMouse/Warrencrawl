using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthLevel
{
    Dictionary<CellCoordinate, LabyrinthCell> cellMap { get; set; }
    Dictionary<CellCoordinate, List<CellCoordinate>> neighborMap { get; set; }

    public List<LabyrinthCell> Map;

    public void UnrollMap()
    {
        cellMap = new Dictionary<CellCoordinate, LabyrinthCell>();
        foreach (LabyrinthCell cell in Map)
        {
            cellMap.Add(cell.Coordinate, cell);
        }

        neighborMap = new Dictionary<CellCoordinate, List<CellCoordinate>>();
        foreach (LabyrinthCell cell in Map)
        {
            List<CellCoordinate> neighbors = new List<CellCoordinate>();
            foreach (CellCoordinate neighbor in cell.Coordinate.Neighbors)
            {
                if (cellMap.ContainsKey(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
            neighborMap.Add(cell.Coordinate, neighbors);
        }
    }
    
    public LabyrinthCell CellAtCoordinate(CellCoordinate coords)
    {
        if (cellMap.TryGetValue(coords, out LabyrinthCell atCoord))
        {
            return atCoord;
        }
        return null;
    }
}
