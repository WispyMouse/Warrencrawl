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
    public event EventHandler LockingAnimationFinished;

    /// <summary>
    /// Pointer to the active LabyrinthSceneHelperTools. Retrieved on Load.
    /// </summary>
    public LabyrinthSceneHelperTools HelperTools { get; private set; }

    /// <summary>
    /// Pointer to the input handler for this state. Retrieved on load from LabyrinthSceneHelperTools.
    /// </summary>
    public LabyrinthInputHandler InputHandler { get; private set; }

    /// <summary>
    /// Pointer to an Animation Handler for this state.
    /// </summary>
    public LabyrinthAnimationHandler AnimationHandler { get; private set; }

    public CombatClock ActiveCombatClock { get; private set; }

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
        AnimationHandler = HelperTools.AnimationHandler;

        LevelToLoad = GameLevelProvider.GetLevel();

        if (LevelToLoad == null)
        {
            Debug.LogWarning($"No {nameof(LevelToLoad)} detected. Either pass a {nameof(GameLevel)} to the {nameof(LabyrinthLevel)} constructor, or have a {nameof(LabyrinthSceneHelperTools.DefaultLevel)} set in {nameof(LabyrinthSceneHelperTools)}.");
            yield break;
        }

        if (LevelToLoad.Scene == null)
        {
            Debug.LogWarning("No Scene detected for the provided LevelToLoad.");
        }
        else
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

        ActiveCombatClock = new CombatClock();
        PointOfViewInstance = GameObject.FindObjectOfType<PointOfView>();
        PointOfViewInstance.CurFacing = Direction.North;
        PointOfViewInstance.CurCoordinates = CellCoordinates.Origin;
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        yield return base.StartState(globalStateMachine, previousState);

        switch (LevelToLoad.CombatClockEnabled)
        {
            case true:
                ActiveCombatClock.ResetCombatClock();
                ActiveCombatClock.Enable();
                break;
            case false:
                ActiveCombatClock.Disable();
                break;
        }

        LabyrinthCell cellAtStart = LevelToLoad.LabyrinthData.CellAtCoordinate(PointOfViewInstance.CurCoordinates);

        if (cellAtStart == null)
        {
            Debug.LogError($"The starting tile at {PointOfViewInstance.CurCoordinates} doesn't exist in the level's map.");
        }

        PointOfViewInstance.transform.rotation = Quaternion.Euler(0, PointOfViewInstance.CurFacing.Degrees(), 0);
        PointOfViewInstance.transform.position = cellAtStart.Worldspace;

        yield return SceneHelperInstance.TransitionsInstance.ContinueTransitionYieldUntilInputsOK();
    }

    public override IEnumerator ExitState(IGameplayState nextState)
    {
        if (nextState != null)
        {
            yield return SceneHelperInstance.TransitionsInstance.TransitionIn();
        }

        yield return base.ExitState(nextState);

        if (LoadedScene.HasValue)
        {
            yield return Addressables.UnloadSceneAsync(LoadedScene.Value);
        }
    }

    public override IEnumerator TransitionUp(IGameplayState nextState)
    {
        yield return base.TransitionUp(nextState);

        yield return SceneHelperInstance.TransitionsInstance.TransitionIn();
    }

    public override void SetControls(WarrencrawlInputs controls)
    {
        controls.Labyrinth.Enable();
        controls.Labyrinth.SetCallbacks(InputHandler);
    }

    /// <summary>
    /// Attempts to move in the direction provided.
    /// This also processes any events that should happen as a result of moving.
    /// </summary>
    /// <param name="offset">Direction to move.</param>
    public IEnumerator Step(Vector3Int offset)
    {
        CellCoordinates newCoordinates = PointOfViewInstance.CurCoordinates + offset;

        LabyrinthCell cellAtPosition = LevelToLoad.LabyrinthData.CellAtCoordinate(newCoordinates);

        if (cellAtPosition == null)
        {
            Debug.Log("Couldn't move there, the cell doesn't exist.");
            LockingAnimationFinished?.Invoke(this, new EventArgs());
            yield break;
        }

        if (!cellAtPosition.Walkable)
        {
            Debug.Log("Couldn't move there, the cell is not walkable.");
            LockingAnimationFinished?.Invoke(this, new EventArgs());
            yield break;
        }

        yield return AnimationHandler.StepTo(PointOfViewInstance, cellAtPosition);

        PointOfViewInstance.CurCoordinates = newCoordinates;

        ActiveCombatClock.StepTaken();

        if (ActiveCombatClock.ShouldEncounterStart())
        {
            yield return StateMachineInstance.PushNewState(new BattleState());
        }

        LockingAnimationFinished?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Rotates the point of view towards the direction provided.
    /// </summary>
    /// <param name="newFacing">The new direction to face.</param>
    public IEnumerator Rotate(Direction newFacing)
    {
        yield return AnimationHandler.Rotate(PointOfViewInstance, newFacing);

        PointOfViewInstance.CurFacing = newFacing;
        LockingAnimationFinished?.Invoke(this, new EventArgs());
        yield break;
    }

    /// <summary>
    /// Attempt to interact with the object in front of the player's view.
    /// </summary>
    public IEnumerator Interact()
    {
        CellCoordinates coordinatesInFacing = PointOfViewInstance.CurCoordinates + PointOfViewInstance.CurFacing.Forward();
        LabyrinthCell cell = LevelToLoad.LabyrinthData.CellAtCoordinate(coordinatesInFacing);

        if (cell == null)
        {
            LockingAnimationFinished?.Invoke(this, new EventArgs());
            yield break;
        }

        if (cell.Interactive == null)
        {
            LockingAnimationFinished?.Invoke(this, new EventArgs());
            yield break;
        }

        // todo: other kinds of interactives!
        switch (cell.Interactive.Kind)
        {
            case InteractiveKind.Stairs:
                yield return StateMachineInstance.ChangeToState(new TownState());
                break;
            default:
                Debug.Log($"Interactive kind is not implemented: {cell.Interactive.Kind}");
                LockingAnimationFinished?.Invoke(this, new EventArgs());
                break;
        }
    }
}
