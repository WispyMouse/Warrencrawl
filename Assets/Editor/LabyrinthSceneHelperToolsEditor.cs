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

    void OnEnable()
    {
        currentLevel = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.CurrentLevel));
        blocked = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.Blocked));
        walkable = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.Walkable));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(blocked);
        EditorGUILayout.PropertyField(walkable);

        EditorGUILayout.PropertyField(currentLevel);

        EditorGUI.BeginChangeCheck();

        GameLevel castLevel = currentLevel.objectReferenceValue as GameLevel;

        if (castLevel != null)
        {
            showCells = EditorGUILayout.Toggle("Show Map Gizmos", showCells);

            if (GUILayout.Button("Scan Level"))
            {
                castLevel.LabyrinthData = ScanLevel();
                
            }
        }

        EditorGUI.EndChangeCheck();

        serializedObject.ApplyModifiedProperties();
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmo(LabyrinthSceneHelperTools editorScript, GizmoType gizmoType)
    {
        if (showCells && editorScript.CurrentLevel != null)
        {
            foreach (LabyrinthCell curCell in editorScript.CurrentLevel.LabyrinthData.Cells)
            {
                if (curCell.Walkable)
                {
                    Gizmos.color = new Color(.5f, 1f, 1f, .6f);
                    Gizmos.DrawCube(new Vector3(curCell.Coordinate.X, 0, curCell.Coordinate.Y), Vector3.one);
                }
                else
                {
                    Gizmos.color = new Color(1f, 0f, 0f, .6f);
                    Gizmos.DrawCube(new Vector3(curCell.Coordinate.X, 0, curCell.Coordinate.Y), Vector3.one);
                }
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
        HashSet<CellCoordinates> visited = new HashSet<CellCoordinates>();
        frontier.Enqueue(CellCoordinates.Origin);

        while (frontier.Any())
        {
            CellCoordinates curFront = frontier.Dequeue();

            if (visited.Contains(curFront))
            {
                continue;
            }

            visited.Add(curFront);

            if (Physics.Linecast(new Vector3(curFront.X, 10, curFront.Y), new Vector3(curFront.X, -10, curFront.Y), blocked.intValue))
            {
                newLevel.Cells.Add(new LabyrinthCell() { Coordinate = curFront, Walkable = false });
            }
            else if (Physics.Linecast(new Vector3(curFront.X, 10, curFront.Y), new Vector3(curFront.X, -10, curFront.Y), walkable.intValue))
            {
                newLevel.Cells.Add(new LabyrinthCell() { Coordinate = curFront, Walkable = true });
            }
            else
            {
                // There was no tile in this space, so let's stop looking here and at its neighbors
                continue;
            }

            foreach (CellCoordinates curNeighbor in curFront.OrthogonalNeighbors)
            {
                if (!visited.Contains(curNeighbor))
                {
                    frontier.Enqueue(curNeighbor);
                }
            }
        }

        return newLevel;
    }
}
