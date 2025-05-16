using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoxShelf))]
[CanEditMultipleObjects]
public class BoxShelfEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BoxShelf boxShelf = target as BoxShelf;
        if (GUILayout.Button("Randomize Box Rotations"))
        {
            Undo.RecordObject(boxShelf.transform, "Randomize Box Rotations");
            boxShelf.RandomizeBoxes();
            PrefabUtility.RecordPrefabInstancePropertyModifications(boxShelf.transform);
        }
    }
}
