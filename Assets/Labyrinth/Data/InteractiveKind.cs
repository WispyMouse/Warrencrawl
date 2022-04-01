using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveKind
{
    /// <summary>
    /// This isn't an Interactive element.
    /// If an object has an InteractiveData with the InteractiveKind set to <see cref="None"/>, it should be ignored.
    /// </summary>
    None,

    /// <summary>
    /// This element is a set of stairs, which will change the floor you're on.
    /// </summary>
    Stairs,

    /// <summary>
    /// You interact with it by standing on its Cell and pressing interact.
    /// </summary>
    SameTileInteractive,

    /// <summary>
    /// You interact with it by standing on a neighboring cell, facing it, and pressing interact.
    /// </summary>
    OutsideTileInteractive
}
