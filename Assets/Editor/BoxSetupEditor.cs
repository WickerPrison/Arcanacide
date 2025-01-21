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
            boxSetup.transform.localRotation = Quaternion.Euler(RandomAngle(), RandomAngle(), RandomAngle());
            PrefabUtility.RecordPrefabInstancePropertyModifications(boxSetup.transform);
        }
    }

    float RandomAngle()
    {
        int randInt = Random.Range(0, 4);
        return randInt * 90f;
    }
}
