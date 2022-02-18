using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// When a LabyrinthLevel is created, is it safe to get null?
    /// </summary>
    [Test]
    public void LabyrinthLevel_CellAtCoordinate_CanGetNull()
    {
        LabyrinthLevel testLevel = new LabyrinthLevel();

        Assert.That(testLevel.CellAtCoordinate(CellCoordinates.Zero), Is.Null);
        Assert.That(testLevel.Cells, Is.Empty);
    }

    /// <summary>
    /// When a LabyrinthLevel is created, is it safe to get the neighbors of a missing coordinate?
    /// </summary>
    [Test]
    public void LabyrinthLevel_NeighborsOfCoordinate_ReturnsEmpty()
    {
        LabyrinthLevel testLevel = new LabyrinthLevel();

        Assert.That(testLevel.NeighborsOfCoordinate(CellCoordinates.Zero), Is.Empty);
    }

    /// <summary>
    /// When a LabyrinthLevel is created, is it safe to get the neighbors of its only cell? Is the list of neighbors empty?
    /// </summary>
    [Test]
    public void Labyrinthlevel_NeighborsOfCoordinate_ReturnsEmptyForExistingCoordinate()
    {
        LabyrinthLevel testLevel = new LabyrinthLevel();

        testLevel.Cells.Add(new LabyrinthCell() { Coordinate = CellCoordinates.Zero });

        Assert.That(testLevel.NeighborsOfCoordinate(CellCoordinates.Zero), Is.Empty);
    }

    /// <summary>
    /// When a LabyrinthLevel is created, are neighbors returned as expected through NeighborsOfCoordinate?
    /// </summary>
    [Test]
    public void Labyrinthlevel_NeighborsOfCoordinate_ReturnsExpectedCoordinates()
    {
        LabyrinthLevel testLevel = new LabyrinthLevel();

        testLevel.Cells.Add(new LabyrinthCell() { Coordinate = CellCoordinates.Zero });
        testLevel.Cells.Add(new LabyrinthCell() { Coordinate = new CellCoordinates(1, 0, 0) });
        testLevel.Cells.Add(new LabyrinthCell() { Coordinate = new CellCoordinates(1, 1, 0) });

        Assert.That(testLevel.NeighborsOfCoordinate(new CellCoordinates(1, 0, 0)), Does.Contain(new CellCoordinates(1, 1, 0)));
        Assert.That(testLevel.NeighborsOfCoordinate(new CellCoordinates(1, 0, 0)), Does.Contain(new CellCoordinates(0, 0, 0)));
        Assert.That(testLevel.NeighborsOfCoordinate(new CellCoordinates(1, 0, 0)).Count(), Is.EqualTo(2));

        Assert.That(testLevel.NeighborsOfCoordinate(new CellCoordinates(1, 1, 0)), Does.Contain(new CellCoordinates(1, 0, 0)));
        Assert.That(testLevel.NeighborsOfCoordinate(new CellCoordinates(1, 1, 0)), Does.Not.Contains(new CellCoordinates(0, 0, 0)));
        Assert.That(testLevel.NeighborsOfCoordinate(new CellCoordinates(1, 1, 0)).Count(), Is.EqualTo(1));
    }
}
