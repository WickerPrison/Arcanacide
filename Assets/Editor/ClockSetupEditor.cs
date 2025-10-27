using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ClockSetup))]
public class ClockSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ClockSetup clockSetup = target as ClockSetup;
        base.OnInspectorGUI();
        if (GUILayout.Button("Randomize Clock"))
        {
            Undo.RecordObject(clockSetup.transform, "Randomize clock");
            clockSetup.RandomizeClock();
            EditorUtility.SetDirty(clockSetup.transform);
            EditorSceneManager.MarkSceneDirty(clockSetup.gameObject.scene);
            PrefabUtility.RecordPrefabInstancePropertyModifications(clockSetup.transform);
        }
    }
}
