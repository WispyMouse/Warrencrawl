using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownStateTools : SceneBootstrapperTools
{ 
    public void ToTheLabyrinth()
    {
        ActiveBootstrapper.StartCoroutine(ActiveStateMachine.ChangeToState(new LabyrinthState()));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new TownState();
    }
}
