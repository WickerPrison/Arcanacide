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
            Object[] stalagmites = targets;
            foreach(Object targetObject in stalagmites)
            {
                IceStalagmite stalagmite = (IceStalagmite)targetObject;
                Undo.RecordObject(stalagmite, "Randomize stalagmite visual");
                stalagmite.RandomizeStalagmite();
                PrefabUtility.RecordPrefabInstancePropertyModifications(stalagmite);
            }
        }
    }
}
