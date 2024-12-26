using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomSetupScript))]
public class RoomSetupEditor : Editor
{


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update NavMesh"))
        {
            Debug.Log("Update navmesh");
        }
    }
}
