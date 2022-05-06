using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSceneHelperTools : SceneHelperTools
{ 
    public void ToTheLabyrinth()
    {
        SceneHelperInstance.PlayCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState( new LabyrinthState(new LabyrinthSceneHelperGrabber())));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new TownState();
    }
}
