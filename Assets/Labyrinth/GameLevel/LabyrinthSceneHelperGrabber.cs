using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This implementation of IGameLevelProvider finds the active <see cref="LabyrinthSceneHelperTools"/>, and asks that it fulfill the level.
/// This is useful because GetLevel should not be called until after the Labyrinth scene is open, and so there will be LabyrinthSceneHelperTools around.
/// </summary>
public class LabyrinthSceneHelperGrabber : IGameLevelProvider
{
    public GameLevel GetLevel()
    {
        LabyrinthSceneHelperTools tools = GameObject.FindObjectOfType<LabyrinthSceneHelperTools>();

        if (tools == null)
        {
            Debug.LogError($"No {nameof(LabyrinthSceneHelperTools)} found.");
            return null;
        }

        return tools.GetLevel();
    }
}
