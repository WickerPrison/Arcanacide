using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoxSetup))]
[CanEditMultipleObjects]
public class BoxSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BoxSetup boxSetup = target as BoxSetup;
        if (GUILayout.Button("Randomize Rotation"))
        {
            Undo.RecordObject(boxSetup.transform, "Randomize Box Rotation");
            boxSetup.RandomRotateBox();
            PrefabUtility.RecordPrefabInstancePropertyModifications(boxSetup.transform);
        }
    }
}
