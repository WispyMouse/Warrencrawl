using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSceneHelperTools : SceneHelperTools
{ 
    public void ToTheLabyrinth()
    {
        SceneHelperInstance.StartCoroutine(SceneHelperInstance.GlobalStateMachineInstance.ChangeToState( new LabyrinthState(SceneHelperInstance, new LabyrinthSceneHelperGrabber())));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new TownState();
    }
}
