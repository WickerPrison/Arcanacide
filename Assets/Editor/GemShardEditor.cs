using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RefundShard))]
public class GemShardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RefundShard gemShard = target as RefundShard;
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate GUID"))
        {
            Undo.RecordObject(gemShard, "Generate GUID");
            gemShard.GenerateGUID();
            PrefabUtility.RecordPrefabInstancePropertyModifications(gemShard);
        }
    }
}
