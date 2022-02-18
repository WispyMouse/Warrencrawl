using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    /// <summary>
    /// When a labyrinth is created with only one cell intended for it, it has exactly only one cell intended for it.
    /// </summary>
    [Test]
    public void LabyrinthLevel_AddsOnlyExpectedCells()
    {
        LabyrinthLevel testLevel = new LabyrinthLevel();

        testLevel.Cells = new List<LabyrinthCell>() 
        { 
            new LabyrinthCell() 
            { 
                Coordinate = CellCoordinates.Zero
            }
        };

        Assert.That(testLevel.CellAtCoordinate(new CellCoordinates(0, 0, 0)), Is.Not.Null);
        Assert.That(testLevel.CellAtCoordinate(new CellCoordinates(0, 0, 0)).Coordinate == CellCoordinates.Zero, Is.Not.Null);
        Assert.That(testLevel.CellAtCoordinate(new CellCoordinates(0, 0, 1)), Is.Null);
        Assert.That(testLevel.CellAtCoordinate(new CellCoordinates(1, -1, 0)), Is.Null);
        Assert.That(testLevel.Cells.Count, Is.EqualTo(1));
    }

    /// <summary>
    /// When a LabyrinthLevel data structure is created, is it safe to get null?
    /// </summary>
    [Test]
    public void LabyrinthLevel_CanGetNull()
    {
        LabyrinthLevel testLevel = new LabyrinthLevel();

        Assert.That(testLevel.CellAtCoordinate(CellCoordinates.Zero), Is.Null);
        Assert.That(testLevel.Cells, Is.Empty);
    }
}
