using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IceStalagmite))]
[CanEditMultipleObjects]
public class IceStalagmiteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Randomize Icicle"))
        {
            var stalagmites = targets;
            foreach(var thing in stalagmites)
            {
                IceStalagmite stalagmite = (IceStalagmite)thing;
                Undo.RecordObject(stalagmite, "Randomize stalagmite visual");
                stalagmite.RandomizeStalagmite();
                PrefabUtility.RecordPrefabInstancePropertyModifications(stalagmite);
            }
        }
    }
}
