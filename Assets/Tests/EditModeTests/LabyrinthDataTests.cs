using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
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
}
