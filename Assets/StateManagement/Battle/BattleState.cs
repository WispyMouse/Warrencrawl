using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : SceneLoadingGameplayState
{
    public override string SceneName => "Battle";
    public override string[] InputMapNames => new string[] { "UI" };
}
