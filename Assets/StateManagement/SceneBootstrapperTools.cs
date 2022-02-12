using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Supports <seealso cref="SceneHelper"/> in wiring all of the game's scenes together.
/// If an inheritor loads in and <seealso cref="SceneHelper"/> isn't loaded, this loads its scene.
/// Then <seealso cref="SceneHelper"/> looks for a class inheriting from this, and uses its <see cref="GetNewDemoState"/> return to start up the <see cref="GlobalStateMachine"/>.
/// </summary>
public abstract class SceneBootstrapperTools : MonoBehaviour
{
    protected SceneHelper SceneHelperInstance { get; private set; }

    private void Start()
    {
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        if (!SceneManager.GetAllScenes().Any(sc => sc.name == "SceneHelper"))
        {
            AsyncOperation bootstrapperScene = SceneManager.LoadSceneAsync("SceneHelper", LoadSceneMode.Additive);
            while (!bootstrapperScene.isDone)
            {
                yield return null;
            }
        }

        SceneHelperInstance = GameObject.FindObjectOfType<SceneHelper>();
    }

    /// <summary>
    /// If this scene was loaded at the start, what gameplay state will this set things to?
    /// This *shouldn't* be utilized in production, but useful for setting up testing environments.
    /// </summary>
    /// <returns></returns>
    public abstract IGameplayState GetNewDemoState();
}
