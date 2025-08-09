using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HalfGolemToggles))]
public class HalfGolemToggleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        HalfGolemToggles halfGolemToggles = target as HalfGolemToggles;
        if (GUILayout.Button("Hide ice chunks"))
        {
            Undo.RecordObject(halfGolemToggles, "Hide Half Golem Ice Chunks");
            halfGolemToggles.ShowIceChunks(false);
            PrefabUtility.RecordPrefabInstancePropertyModifications(halfGolemToggles);
        }
        if (GUILayout.Button("Show ice chunks"))
        {
            Undo.RecordObject(halfGolemToggles, "Show Half Golem Ice Chunks");
            halfGolemToggles.ShowIceChunks(true);
            PrefabUtility.RecordPrefabInstancePropertyModifications(halfGolemToggles);
        }
        if (GUILayout.Button("Enable IK"))
        {
            Undo.RecordObject(halfGolemToggles, "Enable Half Golem IK");
            halfGolemToggles.EnableIK(true);
            PrefabUtility.RecordPrefabInstancePropertyModifications(halfGolemToggles);
        }
        if (GUILayout.Button("Disable IK"))
        {
            Undo.RecordObject(halfGolemToggles, "Disable Half Golem IK");
            halfGolemToggles.EnableIK(false);
            PrefabUtility.RecordPrefabInstancePropertyModifications(halfGolemToggles);
        }
        if (GUILayout.Button("Reset Lazy Values"))
        {
            Undo.RecordObject(halfGolemToggles, "Reset Half Golem Toggles");
            halfGolemToggles.ResetValues();
            PrefabUtility.RecordPrefabInstancePropertyModifications(halfGolemToggles);
        }
    }
}
