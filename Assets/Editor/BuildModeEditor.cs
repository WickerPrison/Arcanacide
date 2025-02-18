using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(BuildMode)), CanEditMultipleObjects]
public class BuildModeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BuildMode buildMode = target as BuildMode;
        base.OnInspectorGUI();
        if (GUILayout.Button("Set Scenes in Build"))
        {
            switch (buildMode.buildMode)
            {
                case BuildModes.DEMO:
                    SetDemoScenes(buildMode); 
                    break;
                default:
                    SetAllScenes();
                    break;
            }
        }
    }

    void SetDemoScenes(BuildMode buildMode)
    {
        EditorBuildSettingsScene[] editorBuildSettingsScenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in editorBuildSettingsScenes)
        {
            if (buildMode.excludePathsForDemo.Any(scene.path.Contains))
            {
                scene.enabled = false;
            }
        }
        EditorBuildSettings.scenes = editorBuildSettingsScenes;
    }

    void SetAllScenes()
    {
        EditorBuildSettingsScene[] editorBuildSettingsScenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in editorBuildSettingsScenes)
        {
            scene.enabled = true;
        }
        EditorBuildSettings.scenes = editorBuildSettingsScenes;
    }
}
