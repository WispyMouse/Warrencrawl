using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // TODO: The scene name to load for this level
    // TODO: A more permanent address per level, or at least one not tied to display name

    /// <summary>
    /// The labyrinth layout for gameplay.
    /// This drives most of the movement engine in the game; the level will follow what this structure has, not what is in the scene.
    /// </summary>
    public LabyrinthLevel LabyrinthData = new LabyrinthLevel();
}
