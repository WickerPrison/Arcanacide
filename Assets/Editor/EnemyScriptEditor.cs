using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyScript))]
public class EnemyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyScript enemyScript = target as EnemyScript;
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate GUID"))
        {
            Undo.RecordObject(enemyScript, "Generate GUID");
            enemyScript.GenerateGUID();
        }
    }
}
