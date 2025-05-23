using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IceStalagmite))]
public class IceStalagmiteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        IceStalagmite stalagmite = target as IceStalagmite;
        if (GUILayout.Button("Randomize Icicle"))
        {
            Undo.RecordObject(stalagmite, "Randomize stalagmite visual");
            stalagmite.RandomizeStalagmite();
            PrefabUtility.RecordPrefabInstancePropertyModifications(stalagmite);
        }
    }
}
