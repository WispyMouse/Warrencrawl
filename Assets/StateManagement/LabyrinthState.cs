using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthState : SceneLoadingGameplayState
{
    public override string SceneName => "Labyrinth";
    public override string[] InputMapNames => new string[] { "UI" };
}
