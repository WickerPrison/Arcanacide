using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelColor
{
    RED, YELLOW, BLUE, PURPLE
}

[CreateAssetMenu]
public class ColorData : ScriptableObject
{
    public Color red;
    public Color yellow;
    public Color blue;
    public Color purple;

    Dictionary<LevelColor, Color> _floorColorDict;
    Dictionary<LevelColor, Color> floorColorDict
    {
        get
        {
            if(_floorColorDict == null)
            {
                _floorColorDict = new Dictionary<LevelColor, Color>
                {
                    { LevelColor.RED, red },
                    { LevelColor.YELLOW, yellow },
                    { LevelColor.BLUE, blue },
                    { LevelColor.PURPLE, purple }
                };
            }
            return _floorColorDict;
        }
    }

    public Color GetColor(LevelColor floorColor)
    {
        return floorColorDict[floorColor];
    }
}
