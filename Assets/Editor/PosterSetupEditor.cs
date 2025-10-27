using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PosterSetup))]
public class PosterSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PosterSetup posterSetup = target as PosterSetup;
        base.OnInspectorGUI();
        if (GUILayout.Button("Randomize Poster"))
        {
            Undo.RecordObject(posterSetup, "Randomize poster");
            posterSetup.Randomize();
            PrefabUtility.RecordPrefabInstancePropertyModifications(posterSetup);
        }
    }
}
