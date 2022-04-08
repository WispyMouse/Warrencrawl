using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownState : SceneLoadingGameplayState
{
    public override string SceneName => "Town";

    public override void SetControls(WarrencrawlInputs controls)
    {
        controls.UI.Enable();
    }

    public override void UnsetControls(WarrencrawlInputs activeInput)
    {
        activeInput.UI.Disable();
    }
}
