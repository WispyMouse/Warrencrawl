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
    SerializedProperty defaultLevel;

    void OnEnable()
    {
        currentLevel = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.CurrentLevel));

        blocked = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.Blocked));
        walkable = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.Walkable));
        defaultLevel = serializedObject.FindProperty(nameof(LabyrinthSceneHelperTools.DefaultLevel));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(blocked);
        EditorGUILayout.PropertyField(walkable);
        EditorGUILayout.PropertyField(defaultLevel);

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
            if (Physics.Raycast(new Vector3(curFront.X, 3f, curFront.Y), Vector3.down, out hit, 4f, blocked.intValue | walkable.intValue))
            {
                bool isWalkable = hit.collider.gameObject.layer == walkable.intValue;

                newLevel.Cells.Add(new LabyrinthCell() { Coordinate = curFront, Height = hit.point.y, Walkable = isWalkable });
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

        return newLevel;
    }
}
