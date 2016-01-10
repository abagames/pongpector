using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Status))]
public class StatusInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var status = target as Status;
        EditorGUILayout.IntField("Score", status.score);
        EditorGUILayout.IntField("Ball", status.ball);
        if (GUILayout.Button("Restart"))
        {
            status.Restart();
        }
    }
}
