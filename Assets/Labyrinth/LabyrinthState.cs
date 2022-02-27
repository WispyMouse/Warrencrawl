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

    /// <summary>
    /// The GameLevel to initialize and load in.
    /// If this is null, we try to pull a default level from <see cref="LabyrinthSceneHelperTools"/>.
    /// </summary>
    public GameLevel LevelToLoad { get; private set; }

    /// <summary>
    /// An object to run IEnumerators against.
    /// </summary>
    public MonoBehaviour Animator { get; private set; }

    /// <summary>
    /// A reference to the loaded scene's instance.
    /// This is used to later unload the scene.
    /// </summary>
    private SceneInstance? LoadedScene { get; set; }

    /// <summary>
    /// The current game object representing the player's point of view.
    /// </summary>
    public PointOfView PointOfViewInstance { get; private set; }

    /// <summary>
    /// A mechanism for providing a level.
    /// </summary>
    private IGameLevelProvider GameLevelProvider { get; set; }

    /// <summary>
    /// Mechanism for interfacing with <see cref="WarrencrawlInputs"/>.
    /// </summary>
    private LabyrinthInputHandler InputHandler { get; set; }

    /// <summary>
    /// Constructor for LabyrinthState.
    /// </summary>
    /// <param name="animator">Any monobehavior able ot run coroutines.</param>
    /// <param name="levelProvider">A mechanism for providing a level.</param>
    public LabyrinthState(MonoBehaviour animator, IGameLevelProvider levelProvider)
    {
        this.Animator = animator;
        this.GameLevelProvider = levelProvider;
    }

    public override IEnumerator Load()
    {
        yield return base.Load();

        LevelToLoad = GameLevelProvider.GetLevel();
        InputHandler = new LabyrinthInputHandler(this);

        if (LevelToLoad == null || LevelToLoad.Scene == null)
        {
            Debug.LogWarning($"No {nameof(LevelToLoad)} detected. Either pass a {nameof(GameLevel)} to the {nameof(LabyrinthLevel)} constructor, or have a {nameof(LabyrinthSceneHelperTools.DefaultLevel)} set in {nameof(LabyrinthSceneHelperTools)}.");
            yield break;
        }

        var loc = Addressables.LoadResourceLocationsAsync(LevelToLoad.Scene);
        yield return loc;
        if (!SceneManager.GetSceneByPath(loc.Result[0].InternalId).isLoaded)
        {
            AsyncOperationHandle<SceneInstance> loadingOperation = Addressables.LoadSceneAsync(LevelToLoad.Scene, LoadSceneMode.Additive);
            yield return loadingOperation;

            LoadedScene = loadingOperation.Result;
        }

        PointOfViewInstance = GameObject.FindObjectOfType<PointOfView>();
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        yield return base.StartState(globalStateMachine, previousState);

        // TODO: Get POV coordinates somehow; probably going to be the labyrinthscenetools again
        PointOfViewInstance.CurFacing = Direction.North;
        PointOfViewInstance.CurCoordinates = CellCoordinates.Origin;
    }

    public override IEnumerator ExitState(IGameplayState nextState)
    {
        yield return base.ExitState(nextState);

        if (LoadedScene.HasValue)
        {
            yield return Addressables.UnloadSceneAsync(LoadedScene.Value);
        }
    }

    public override void SetControls(WarrencrawlInputs controls)
    {
        controls.Labyrinth.Enable();
        controls.Labyrinth.SetCallbacks(InputHandler);
    }

    /// <summary>
    /// Attempts to move in the direction provided.
    /// </summary>
    /// <param name="offset">Direction to move.</param>
    public IEnumerator Step(Vector3Int offset)
    {
        CellCoordinates newCoordinates = new CellCoordinates(
            PointOfViewInstance.CurCoordinates.X + offset.x,
            PointOfViewInstance.CurCoordinates.Y + offset.y,
            PointOfViewInstance.CurCoordinates.Z + offset.z);

        LabyrinthCell cellAtPosition = LevelToLoad.LabyrinthData.CellAtCoordinate(newCoordinates);

        if (cellAtPosition == null)
        {
            Debug.Log("Couldn't move there.");
            yield break;
        }

        PointOfViewInstance.transform.position = cellAtPosition.Worldspace;
        PointOfViewInstance.CurCoordinates = newCoordinates;
    }

    public IEnumerator Rotate(Direction newFacing)
    {
        PointOfViewInstance.CurFacing = newFacing;
        PointOfViewInstance.transform.rotation = Quaternion.Euler(0, newFacing.Degrees(), 0);
        yield break;
    }
}
