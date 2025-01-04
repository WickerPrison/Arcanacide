using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomSetupScript))]
public class RoomSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomSetupScript roomSetup = target as RoomSetupScript;
        base.OnInspectorGUI();
        if(GUILayout.Button("Manual Update"))
        {
            roomSetup.ManualUpdate();
        }

        if (roomSetup.usesNavmesh)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear NavMesh"))
            {
                roomSetup.ClearNavmesh();
            }
            if (GUILayout.Button("Bake NavMesh"))
            {
                roomSetup.UpdateNavmesh();
            }
            GUILayout.EndHorizontal();
        }
    }
}
