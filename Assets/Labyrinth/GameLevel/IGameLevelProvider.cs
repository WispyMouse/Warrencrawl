using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for things that can provide game levels. This is so that you can provide different engines and contexts for level generation.
/// </summary>
public interface IGameLevelProvider
{
    /// <summary>
    /// Gets a level for use.
    /// Should be used after the Labyrinth scene has been opened, so that commonly used hookup tools can be found.
    /// </summary>
    /// <returns>The GameLevel to use.</returns>
    public GameLevel GetLevel();
}
