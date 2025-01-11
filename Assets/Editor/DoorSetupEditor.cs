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
            if(!spawnManager.spawnPoints.Contains(doorSetup.spawnPoint))
            {
                Undo.RecordObject(spawnManager, "Add door to spawnmanager");
                spawnManager.spawnPoints.Add(doorSetup.spawnPoint);
                PrefabUtility.RecordPrefabInstancePropertyModifications(spawnManager);
            }
            doorSetup.gameObject.name = "Doorway " + spawnManager.spawnPoints.IndexOf(doorSetup.spawnPoint).ToString();
        }
    }
}
