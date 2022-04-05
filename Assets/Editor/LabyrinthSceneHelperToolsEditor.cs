using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LabyrinthSceneHelperTools))]
public class LabyrinthSceneHelperToolsEditor : Editor
{
    static bool showCells { get; set; }

    SerializedProperty currentLevel;

    SerializedProperty blocked;
    SerializedProperty walkable;
    SerializedProperty interactive;

    SerializedProperty defaultLevel;

    SerializedProperty inputHandler;
    SerializedProperty animationHandler;

    void OnEnable()
    {
        currentLevel = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.CurrentLevel));

        blocked = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.Blocked));
        walkable = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.Walkable));
        interactive = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.Interactive));

        defaultLevel = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.DefaultLevel));

        inputHandler = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.InputHandler));
        animationHandler = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.AnimationHandler));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(blocked);
        EditorGUILayout.PropertyField(walkable);
        EditorGUILayout.PropertyField(interactive);

        EditorGUILayout.PropertyField(defaultLevel);
        EditorGUILayout.PropertyField(inputHandler);
        EditorGUILayout.PropertyField(animationHandler);

        EditorGUILayout.PropertyField(currentLevel);

        GameLevel castCurrentLevel = ((LabyrinthSceneHelperTools)target).CurrentLevel;

        if (castCurrentLevel != null)
        {
            EditorGUILayout.LabelField("Cells in Map", castCurrentLevel.LabyrinthData.Cells.Count().ToString());

            EditorGUI.BeginChangeCheck();
            showCells = EditorGUILayout.Toggle("Show Map Gizmos", showCells);
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }

            if (GUILayout.Button("Scan Level"))
            {
                castCurrentLevel.LabyrinthData = ScanLevel();
                EditorUtility.SetDirty(castCurrentLevel);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmo(LabyrinthSceneHelperTools editorScript, GizmoType gizmoType)
    {
        if (!(showCells && editorScript.CurrentLevel != null))
        {
            return;
        }

        foreach (LabyrinthCell curCell in editorScript.CurrentLevel.LabyrinthData.Cells)
        {
            Gizmos.color = curCell.DebugColor;
            Gizmos.DrawCube(curCell.Worldspace, Vector3.one);
        }

        foreach (InteractiveData interactive in editorScript.CurrentLevel.LabyrinthData.LabyrinthInteractives)
        {
            foreach (CellCoordinates coordinate in interactive.OnCoordinates)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(editorScript.CurrentLevel.LabyrinthData.CellAtCoordinate(coordinate).Worldspace, .5f);
            }
        }
    }

    /// <summary>
    /// Scans the level's geometry and generates a Labyrinth Level from it.
    /// This starts by going to the origin (0, 0, 0) and scanning each neighboring tile.
    /// Currently that means levels need to be continuous.
    /// </summary>
    /// <returns>A LabyrinthLevel matching the scene.</returns>
    LabyrinthLevel ScanLevel()
    {
        LabyrinthLevel newLevel = new LabyrinthLevel();

        Queue<CellCoordinates> frontier = new Queue<CellCoordinates>();
        HashSet<CellCoordinates> seen = new HashSet<CellCoordinates>();
        frontier.Enqueue(CellCoordinates.Origin);
        seen.Add(CellCoordinates.Origin);

        while (frontier.Any())
        {
            CellCoordinates curFront = frontier.Dequeue();

            RaycastHit hit;
            Collider[] boxColliders = Physics.OverlapBox(new Vector3(curFront.X, 0f, curFront.Y), Vector3.one / 2f, Quaternion.identity, blocked.intValue);

            if (boxColliders.Length > 0)
            {
                LabyrinthCell detectedCell = new LabyrinthCell() { Coordinate = curFront, DefaultWalkable = false };
                newLevel.Cells.Add(detectedCell);
            }
            else if (Physics.Raycast(new Vector3(curFront.X, 3f, curFront.Y), Vector3.down, out hit, 4f, walkable.intValue))
            {
                bool isWalkable = (1 << hit.collider.gameObject.layer) == walkable.intValue;

                LabyrinthCell detectedCell = new LabyrinthCell() { Coordinate = curFront, Height = hit.point.y, DefaultWalkable = isWalkable };

                newLevel.Cells.Add(detectedCell);
            }
            else
            {
                // There was no tile in this space, so let's stop looking here and at its neighbors
                continue;
            }

            foreach (CellCoordinates curNeighbor in curFront.OrthogonalNeighbors)
            {
                if (seen.Contains(curNeighbor))
                {
                    continue;
                }

                seen.Add(curNeighbor);
                frontier.Enqueue(curNeighbor);
            }
        }

        foreach (LabyrinthInteractive processingInteractive in FindObjectsOfType<LabyrinthInteractive>())
        {
            HashSet<CellCoordinates> onCoordinates = new HashSet<CellCoordinates>();

            foreach (Collider interactiveCollider in processingInteractive.GetComponentsInChildren<Collider>())
            {
                int farthestWest = Mathf.FloorToInt(interactiveCollider.bounds.min.x);
                int farthestSouth = Mathf.FloorToInt(interactiveCollider.bounds.min.z);
                int farthestBottom = Mathf.FloorToInt(interactiveCollider.bounds.min.y);

                int farthestEast = Mathf.CeilToInt(interactiveCollider.bounds.max.x);
                int farthestNorth = Mathf.CeilToInt(interactiveCollider.bounds.max.z);
                int farthestTop = Mathf.CeilToInt(interactiveCollider.bounds.max.y);

                for (int xx = farthestWest; xx <= farthestEast; xx++)
                {
                    for (int zz = farthestSouth; zz <= farthestSouth; zz++)
                    {
                        // todo: implement farthestBottom, farthestTop; multiple types of verticality

                        IEnumerable<LabyrinthCell> matchingCells = newLevel.Cells.Where(c => c.Coordinate.X == xx && c.Coordinate.Y == zz);
                        foreach (LabyrinthCell cell in matchingCells)
                        {
                            if (Physics.OverlapBox(cell.Worldspace, Vector3.one / 2f, Quaternion.identity, interactive.intValue).Any(foundCollider => foundCollider == interactiveCollider))
                            {
                                processingInteractive.Data.OnCoordinates.Add(cell.Coordinate);
                            }
                        }
                    }
                }
            }

            newLevel.LabyrinthInteractives.Add(processingInteractive.Data);
        }

        return newLevel;
    }
}
