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

                while (!loadingOperation.IsDone)
                {
                    yield return loadingOperation.PercentComplete;
                }

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
            AsyncOperationHandle unloadingOperation = Addressables.UnloadSceneAsync(LoadedScene.Value);

            while (!unloadingOperation.IsDone)
            {
                yield return unloadingOperation.PercentComplete;
            }
        }
    }
}
