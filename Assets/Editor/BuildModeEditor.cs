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
        string[] paths = buildMode.excludeScenesForDemo.Select(sceneAsset => AssetDatabase.GetAssetPath(sceneAsset)).ToArray();
        EditorBuildSettingsScene[] editorBuildSettingsScenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in editorBuildSettingsScenes)
        {
            if (buildMode.excludePathsForDemo.Any(scene.path.Contains) || paths.Contains(scene.path))
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
