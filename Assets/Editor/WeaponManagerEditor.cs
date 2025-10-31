using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponManager))]
public class WeaponManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WeaponManager weaponManager = target as WeaponManager;
        //base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            weaponManager.InspectorChanged();
        }
    }
}
