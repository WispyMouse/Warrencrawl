using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
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
    /// Reports when an exclusive animation has finished.
    /// </summary>
    public EventHandler LockingAnimationFinished { get; set; }

    /// <summary>
    /// Pointer to the active LabyrinthSceneHelperTools. Retrieved on Load.
    /// </summary>
    public LabyrinthSceneHelperTools HelperTools { get; private set; }

    /// <summary>
    /// Pointer to the input handler for this state. Retrieved on load from LabyrinthSceneHelperTools.
    /// </summary>
    public LabyrinthInputHandler InputHandler { get; private set; }

    /// <summary>
    /// Constructor for LabyrinthState.
    /// </summary>
    /// <param name="levelProvider">A mechanism for providing a level.</param>
    public LabyrinthState(IGameLevelProvider levelProvider)
    {
        this.GameLevelProvider = levelProvider;
    }

    public override IEnumerator Load()
    {
        yield return base.Load();

        HelperTools = GameObject.FindObjectOfType<LabyrinthSceneHelperTools>();

        if (HelperTools == null)
        {
            Debug.LogWarning($"No {nameof(LabyrinthSceneHelperTools)} found in the scene.");
            yield break;
        }

        InputHandler = HelperTools.InputHandler;

        LevelToLoad = GameLevelProvider.GetLevel();

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
            Debug.Log("Couldn't move there, the cell doesn't exist.");
            LockingAnimationFinished.Invoke(this, new EventArgs());
            yield break;
        }

        if (!cellAtPosition.Walkable)
        {
            Debug.Log("Couldn't move there, the cell is not walkable.");
            LockingAnimationFinished.Invoke(this, new EventArgs());
            yield break;
        }

        Vector3 startingPosition = PointOfViewInstance.transform.position;
        Vector3 targetPosition = cellAtPosition.Worldspace;
        float curTime = 0;
        float stepTime = .5f;

        while (curTime < stepTime)
        {
            PointOfViewInstance.transform.position = Vector3.Lerp(startingPosition, targetPosition,  curTime / stepTime);
            curTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        PointOfViewInstance.transform.position = targetPosition;
        PointOfViewInstance.CurCoordinates = newCoordinates;
        LockingAnimationFinished.Invoke(this, new EventArgs());
    }

    public IEnumerator Rotate(Direction newFacing)
    {
        Vector3 startingFacing = PointOfViewInstance.transform.rotation.eulerAngles;
        Vector3 targetFacing = new Vector3(0, newFacing.Degrees(), 0);
        float curTime = 0;
        float turnTime = .5f;

        while (curTime < turnTime)
        {
            PointOfViewInstance.transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(startingFacing.y, targetFacing.y, curTime / turnTime), 0);
            curTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        PointOfViewInstance.CurFacing = newFacing;
        PointOfViewInstance.transform.rotation = Quaternion.Euler(targetFacing);
        LockingAnimationFinished.Invoke(this, new EventArgs());
        yield break;
    }
}
