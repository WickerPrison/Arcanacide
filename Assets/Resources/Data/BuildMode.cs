using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildModes
{
    DEMO, FULLGAME, TESTING
}

[CreateAssetMenu]
public class BuildMode : ScriptableObject
{
    public string versionNumber;
    public BuildModes buildMode;
    public string[] excludePathsForDemo;
    public float testTimeScale;
}
