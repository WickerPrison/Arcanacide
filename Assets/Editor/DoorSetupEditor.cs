using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorSetup))]
public class DoorSetupEditor : Editor
{
    SpawnManager SpawnManager;
    SpawnManager spawnManager
    {
        get
        {
            if(SpawnManager == null)
            {
                SpawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
            }
            return SpawnManager;
        }
    }

    public override void OnInspectorGUI()
    {
        DoorSetup doorSetup = target as DoorSetup;
        base.OnInspectorGUI();
        if (GUILayout.Button("Assign to SpawnManager"))
        {
            Undo.RecordObject(spawnManager, "Add door to spawnmanager");
            if(!spawnManager.spawnPoints.Contains(doorSetup.spawnPoint))
            {
                spawnManager.spawnPoints.Add(doorSetup.spawnPoint);
            }
            doorSetup.gameObject.name = "Doorway " + spawnManager.spawnPoints.IndexOf(doorSetup.spawnPoint).ToString();
        }
    }
}
