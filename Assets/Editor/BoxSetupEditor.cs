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
        if (GUILayout.Button("Randomize Rotation"))
        {
            Object[] boxes = targets;
            foreach(Object targetObject in boxes)
            {
                BoxSetup boxSetup = (BoxSetup)targetObject;
                Undo.RecordObject(boxSetup.transform, "Randomize Box Rotation");
                boxSetup.RandomRotateBox();
                PrefabUtility.RecordPrefabInstancePropertyModifications(boxSetup.transform);
            }
        }
    }
}
