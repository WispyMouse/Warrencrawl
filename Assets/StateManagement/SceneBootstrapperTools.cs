using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneBootstrapperTools : MonoBehaviour
{
    protected Bootstrapper ActiveBootstrapper { get; private set; }
    protected GlobalStateMachine ActiveStateMachine { get; private set; }

    private void Start()
    {
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        if (!SceneManager.GetAllScenes().Any(sc => sc.name == "Bootstrapper"))
        {
            AsyncOperation bootstrapperScene = SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Additive);
            while (!bootstrapperScene.isDone)
            {
                yield return null;
            }
        }

        Bootstrapper existingBootstrapper = GameObject.FindObjectOfType<Bootstrapper>();
        ActiveStateMachine = existingBootstrapper.GlobalStateMachineInstance;
        ActiveBootstrapper = existingBootstrapper;
    }

    /// <summary>
    /// If this scene was loaded at the start, what gameplay state will this set things to?
    /// This *shouldn't* be utilized in production, but useful for setting up testing environments.
    /// </summary>
    /// <returns></returns>
    public abstract IGameplayState GetNewDemoState();
}
