using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a level of the labyrinth.
/// "Level" is a single conceptual connected space, and doesn't limit it to a single 2D plane.
/// This is focused on the geometrical concepts of the level; where things it are, such as doors and ladders.
/// </summary>
[System.Serializable]
public class LabyrinthLevel
{
    protected Dictionary<CellCoordinates, LabyrinthCell> CellMap
    {
        get
        {
            if (loadedCellMap != null && lastCellsHash == Cells.GetHashCode())
            {
                return loadedCellMap;
            }

            UnrollMap();
            return loadedCellMap;
        }
    }
    private Dictionary<CellCoordinates, LabyrinthCell> loadedCellMap { get; set; }
    
    protected Dictionary<CellCoordinates, List<CellCoordinates>> NeighborMap
    {
        get
        {
            if (loadedNeighborMap != null && lastCellsHash == Cells.GetHashCode())
            {
                return loadedNeighborMap;
            }

            UnrollMap();
            return loadedNeighborMap;
        }
    }
    private Dictionary<CellCoordinates, List<CellCoordinates>> loadedNeighborMap { get; set; }

    protected Dictionary<CellCoordinates, HashSet<InteractiveData>> InteractiveMap
    {
        get
        {
            if (loadedCellMap != null && lastCellsHash == Cells.GetHashCode())
            {
                return loadedInteractiveMap;
            }

            UnrollMap();
            return loadedInteractiveMap;
        }
    }
    private Dictionary<CellCoordinates, HashSet<InteractiveData>> loadedInteractiveMap { get; set; }

    /// <summary>
    /// The data structure for all of the interactives on the map. This is serialized.
    /// </summary>
    public List<InteractiveData> LabyrinthInteractives = new List<InteractiveData>();

    /// <summary>
    /// The data structure for all of the pieces in a level. This is serialized.
    /// </summary>
    public List<LabyrinthCell> Cells = new List<LabyrinthCell>();
    private int lastCellsHash { get; set; }

    /// <summary>
    /// "Unrolls" the <see cref="Cells"/> in to <see cref="loadedCellMap"/> and <see cref="loadedNeighborMap"/>.
    /// This and their property accessors should be the only place talking to loadedCellMap and loadedNeighborMap.
    /// Because the accessors for <see cref="CellMap"/> and <see cref="NeighborMap"/> call this function, this function should *not* call those.
    /// </summary>
    private void UnrollMap()
    {
        loadedCellMap = new Dictionary<CellCoordinates, LabyrinthCell>();
        foreach (LabyrinthCell cell in Cells)
        {
            loadedCellMap.Add(cell.Coordinate, cell);
        }

        loadedNeighborMap = new Dictionary<CellCoordinates, List<CellCoordinates>>();
        foreach (LabyrinthCell cell in Cells)
        {
            List<CellCoordinates> neighbors = new List<CellCoordinates>();
            foreach (CellCoordinates neighbor in cell.Coordinate.OrthogonalNeighbors)
            {
                if (loadedCellMap.ContainsKey(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
            loadedNeighborMap.Add(cell.Coordinate, neighbors);
        }

        loadedInteractiveMap = new Dictionary<CellCoordinates, HashSet<InteractiveData>>();
        foreach (InteractiveData interactive in LabyrinthInteractives)
        {
            foreach (CellCoordinates coordinate in interactive.OnCoordinates)
            {
                if (loadedInteractiveMap.ContainsKey(coordinate))
                {
                    loadedInteractiveMap[coordinate].Add(interactive);
                }
                else
                {
                    loadedInteractiveMap.Add(coordinate, new HashSet<InteractiveData> { interactive });
                }
            }
        }

        lastCellsHash = Cells.GetHashCode();
    }
    
    /// <summary>
    /// Returns a cell at a given coordinate.
    /// This is an exact check; it won't get cells above or below it.
    /// </summary>
    /// <param name="coords">The coordinates to check.</param>
    /// <returns>The LabyrinthCell at the coordinates, if there was one.</returns>
    public LabyrinthCell CellAtCoordinate(CellCoordinates coords)
    {
        if (CellMap.TryGetValue(coords, out LabyrinthCell atCoord))
        {
            return atCoord;
        }
        return null;
    }

    /// <summary>
    /// Returns all neighbors of a coordinate.
    /// This'll check orthogonal neighbors and any special connectors relating to the cell.
    /// </summary>
    /// <param name="centerCoordinate">Coordinate to get the neighbors of.</param>
    /// <returns>A collection of coordinates containing neighbors.</returns>
    public IEnumerable<CellCoordinates> NeighborsOfCoordinate(CellCoordinates centerCoordinate)
    {
        if (NeighborMap.TryGetValue(centerCoordinate, out List<CellCoordinates> neighbors))
        {
            return neighbors;
        }
        return new List<CellCoordinates>();
    }

    public IEnumerable<InteractiveData> InteractivesAtCoordinate(CellCoordinates coordinate)
    {
        if (InteractiveMap.TryGetValue(coordinate, out HashSet<InteractiveData> interactives))
        {
            return interactives;
        }

        return new HashSet<InteractiveData>();
    }
}
