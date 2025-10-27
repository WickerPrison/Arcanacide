using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyScript), true)]
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
            PrefabUtility.RecordPrefabInstancePropertyModifications(enemyScript);
        }
    }

    [MenuItem("Tools/Assign Enemy IDs")]
    public static void AssignEnemyIDs()
    {
        EnemyScript[] enemyScripts = FindObjectsOfType<EnemyScript>();
        foreach(EnemyScript enemy in enemyScripts)
        {
            Undo.RecordObjects(enemyScripts, "Generate GUIDs");
            if(enemy.enemyGUID == "")
            {
                enemy.GenerateGUID();
                PrefabUtility.RecordPrefabInstancePropertyModifications(enemy);
            }
        }
    }
}
