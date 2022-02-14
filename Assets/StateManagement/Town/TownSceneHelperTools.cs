using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSceneHelperTools : SceneHelperTools
{ 
    public void ToTheLabyrinth()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.ChangeToState(new LabyrinthState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new TownState();
    }
}
