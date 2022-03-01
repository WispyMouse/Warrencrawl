using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clock tracking when the next combat encounter should begin.
/// </summary>
public class CombatClock
{
    /// <summary>
    /// Max range for when an encounter should start.
    /// If someone <see cref="LabyrinthState.Step(Vector3Int)"/>s this many times, they'll definitely encounter an enemy.
    /// </summary>
    public int MaxStepsToEncounter { get; set; } = 15;

    /// <summary>
    /// Min range for when an encounter should start.
    /// You'll be able to walk at least this many steps minus one without encountering an enemy.
    /// </summary>
    public int MinStepsToEncounter { get; set; } = 5;

    /// <summary>
    /// Current steps until an encounter begins.
    /// </summary>
    public int StepsToEncounter { get; set; }

    /// <summary>
    /// Sets if the CombatClock should tick down and pop.
    /// </summary>
    public bool Enabled { get; private set; } = true;

    public CombatClock()
    {
        ResetCombatClock();
    }

    public void ResetCombatClock()
    {
        StepsToEncounter = Random.Range(MinStepsToEncounter, MaxStepsToEncounter);
    }

    public void StepTaken()
    {
        if (!Enabled)
        {
            return;
        }

        StepsToEncounter--;
    }

    public bool ShouldEncounterStart()
    {
        return StepsToEncounter <= 0;
    }

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }
}
