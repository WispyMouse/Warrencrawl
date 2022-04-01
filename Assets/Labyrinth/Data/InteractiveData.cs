using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractiveData
{
    public InteractiveKind Kind;
    public bool Walkable
    {
        get
        {
            switch (Kind)
            {
                default:
                case InteractiveKind.None:
                case InteractiveKind.SameTileInteractive:
                    return true;
                case InteractiveKind.OutsideTileInteractive:
                case InteractiveKind.Stairs:
                    return false;
            }
        }
    }

    public string Message;
}
