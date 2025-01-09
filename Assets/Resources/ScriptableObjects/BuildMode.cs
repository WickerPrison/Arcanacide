using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum BuildModes
{
    DEMO, FULLGAME, TESTING
}

[CreateAssetMenu]
public class BuildMode : ScriptableObject
{
    public BuildModes buildMode;
    public string[] excludePathsForDemo;
    public SceneAsset[] excludeScenesForDemo;
}
