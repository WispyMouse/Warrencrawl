using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Represents one level of the labyrinth, in its entireity.
/// This holds things like the name of the level, the events of the level, how to address the level, etc.
/// </summary>
[CreateAssetMenu(fileName = "GameLevel", menuName = "Create Game Level")]
public class GameLevel : ScriptableObject
{
    /// <summary>
    /// The display name for this level.
    /// </summary>
    public string LevelName = "Demo Game Level";

    /// <summary>
    /// The labyrinth layout for gameplay.
    /// This drives most of the movement engine in the game; the level will follow what this structure has, not what is in the scene.
    /// </summary>
    public LabyrinthLevel LabyrinthData = new LabyrinthLevel();

    /// <summary>
    /// A reference to the scene to additively load for this Labyrinth.
    /// If null, no scene is loaded for the Labyrinth, which is likely an error state.
    /// </summary>
    public string Scene;

    /// <summary>
    /// Sets whether to have the <see cref="CombatClock"/> enabled by default.
    /// </summary>
    public bool CombatClockEnabled = true;
}
