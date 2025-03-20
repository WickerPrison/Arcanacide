using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GemShard))]
public class GemShardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GemShard gemShard = target as GemShard;
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate GUID"))
        {
            Undo.RecordObject(gemShard, "Generate GUID");
            gemShard.GenerateGUID();
            PrefabUtility.RecordPrefabInstancePropertyModifications(gemShard);
        }
    }
}
