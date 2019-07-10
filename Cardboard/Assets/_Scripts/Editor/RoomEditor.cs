using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomGenerator))]
public class RoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomGenerator m_script = (RoomGenerator)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generar"))
        {
            m_script.Generator();
        }

        if (GUILayout.Button("Eliminar"))
        {
            m_script.Restar();
        }
    }
}
