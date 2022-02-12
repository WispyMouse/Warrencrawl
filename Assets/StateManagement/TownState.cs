using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownState : SceneLoadingGameplayState
{
    public override string SceneName => "Town";
    public override string[] InputMapNames => new string[] { "UI" };
}
