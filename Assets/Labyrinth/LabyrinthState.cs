using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LabyrinthState : SceneLoadingGameplayState
{
    public override string SceneName => "Labyrinth";
    public override string[] InputMapNames => new string[] { "UI" };

    /// <summary>
    /// The GameLevel to initialize and load in.
    /// If this is null, we try to pull a default level from <see cref="LabyrinthSceneHelperTools"/>.
    /// </summary>
    public GameLevel LevelToLoad { get; private set; }

    /// <summary>
    /// A reference to the loaded scene's instance.
    /// This is used to later unload the scene.
    /// </summary>
    private SceneInstance? LoadedScene { get; set; }

    /// <summary>
    /// Default constructor. <see cref="LevelToLoad"/> is not set, reads from LabyrinthSceneHelperTools to determine a default level to load.
    /// </summary>
    public LabyrinthState()
    {

    }

    /// <summary>
    /// Constructor that sets the level that you're transitioning in to.
    /// </summary>
    /// <param name="toLoad">The level to load.</param>
    public LabyrinthState(GameLevel toLoad)
    {
        LevelToLoad = toLoad;
    }

    public override IEnumerator Load()
    {
        yield return base.Load();

        if (LevelToLoad == null)
        {
            // TODO reduce coupling; can this be passed somehow?
            LabyrinthSceneHelperTools tools = GameObject.FindObjectOfType<LabyrinthSceneHelperTools>();
            LevelToLoad = tools.DefaultLevel;
        }

        if (LevelToLoad != null && LevelToLoad.Scene != null)
        {
            var loc = Addressables.LoadResourceLocationsAsync(LevelToLoad.Scene);
            yield return loc;
            if (!SceneManager.GetSceneByPath(loc.Result[0].InternalId).isLoaded)
            {
                AsyncOperationHandle<SceneInstance> loadingOperation = Addressables.LoadSceneAsync(LevelToLoad.Scene, LoadSceneMode.Additive);
                yield return loadingOperation;

                LoadedScene = loadingOperation.Result;
            }
        }
        else
        {
            Debug.LogWarning($"No {nameof(LevelToLoad)} detected. Either pass a {nameof(GameLevel)} to the {nameof(LabyrinthLevel)} constructor, or have a {nameof(LabyrinthSceneHelperTools.DefaultLevel)} set in {nameof(LabyrinthSceneHelperTools)}.");
        }
    }

    public override IEnumerator ExitState(IGameplayState nextState)
    {
        yield return base.ExitState(nextState);

        if (LoadedScene.HasValue)
        {
            yield return Addressables.UnloadSceneAsync(LoadedScene.Value);
        }
    }
}
