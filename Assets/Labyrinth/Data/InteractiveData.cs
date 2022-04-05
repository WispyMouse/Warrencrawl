using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractiveData
{
    public SerializableGuid InteractiveID;

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

    public List<InteractiveAppearanceCheck> Checks;
    public List<InteractiveSetFlag> FlagSet;

    public HashSet<CellCoordinates> OnCoordinates;

    public bool IsActive { get; set; } = true;
    public LabyrinthInteractive WorldInteractive { get; set; }

    public InteractiveData()
    {
        InteractiveID = Guid.NewGuid();
    }

    public bool ShouldEnableBasedOnSetFlags(SaveData checkedSaveData)
    {
        // If there are no checks, then this is always visible
        if (Checks.Count == 0)
        {
            return true;
        }

        bool shouldBeVisible = true;

        // If every check relates to being shown, then be hidden by default
        // vice versa, if every check relates to being hidden, it should be shown by default
        if (Checks.TrueForAll((InteractiveAppearanceCheck iac) => { return iac.ShouldBeVisible == true; }))
        {
            shouldBeVisible = false;
        }
        else if (Checks.TrueForAll((InteractiveAppearanceCheck iac) => { return iac.ShouldBeVisible == false; }))
        {
            shouldBeVisible = true;
        }

        // This checks in order;
        // if there are three checks, first saying it should be hidden, second not applying, third saying it should be visible, it should be visible
        foreach (InteractiveAppearanceCheck check in Checks)
        {
            if (checkedSaveData.GetFlag(check.FlagNameToCheck) >= 0)
            {
                shouldBeVisible = check.ShouldBeVisible;
            }
        }

        return shouldBeVisible;
    }
}
