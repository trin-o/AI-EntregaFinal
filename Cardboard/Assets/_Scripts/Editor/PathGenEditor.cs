using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathGenerator))]
public class PathGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PathGenerator pathG = (PathGenerator)target;
        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Graph"))
        {
            pathG.GenerateGraph();
        }



        if (pathG.graph != null && pathG.graph.data != null && pathG.graph.data.Length > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Graph:");
            EditorGUILayout.Space();

            GUIStyle rowStyle = new GUIStyle(EditorStyles.textField);
            rowStyle.fontSize = 8;
            rowStyle.margin = new RectOffset();
            // rowStyle.padding = new RectOffset();
            rowStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUIUtility.fieldWidth = 10f;

            EditorGUILayout.BeginVertical();
            Color defaultCol = rowStyle.normal.textColor;

            for (int i = 0; i < pathG.graph.rows; i++)
            {
                EditorGUI.indentLevel = 0;
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < pathG.graph.rows; j++)
                {
                    if (pathG.graph.GetCell(i, j) == 0) rowStyle.normal.textColor = Color.grey;
                    pathG.graph.SetCell(i, j, EditorGUILayout.IntField(pathG.graph.GetCell(i, j), rowStyle));
                    rowStyle.normal.textColor = defaultCol;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
        {
            Undo.RecordObject(pathG, "Edit graph");
            EditorUtility.SetDirty(pathG);
        }
    }
}