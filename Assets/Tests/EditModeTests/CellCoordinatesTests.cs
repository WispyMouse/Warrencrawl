using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CellCoordinatesTests
{
    /// <summary>
    /// Checks that coordinates equal to eachother in ways that are expected.
    /// </summary>
    [Test]
    public void EqualsOperator_AsExpected()
    {
        CellCoordinates a = new CellCoordinates(5, 5, 2);
        CellCoordinates b = new CellCoordinates(5, 5, 3);
        CellCoordinates c = new CellCoordinates(3, 2, 1);
        CellCoordinates d_sameAsA = new CellCoordinates(5, 5, 2);

        Assert.IsTrue(a != b);
        Assert.IsFalse(a == c);
        Assert.IsTrue(a == d_sameAsA);
        Assert.IsFalse(b == d_sameAsA);
        Assert.IsFalse(a != d_sameAsA);
    }

    /// <summary>
    /// Checks that a non-coordinate running Equals will result in false.
    /// </summary>
    [Test]
    public void Equals_NotCoordinate_False()
    {
        CellCoordinates a = new CellCoordinates(2, 2, 2);
        object falseObject = new object();

        Assert.IsFalse(a.Equals(falseObject));
    }

    /// <summary>
    /// Validates the ToString function works as expected.
    /// </summary>
    [Test]
    public void ToString_ExpectedValue()
    {
        CellCoordinates toTest = new CellCoordinates(15, 22, 43);
        Assert.That(toTest.ToString(), Is.EqualTo("(15, 22, 43)"));
    }
}
