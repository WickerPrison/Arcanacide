using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColorChange : MonoBehaviour
{
    public bool colorChange;
    [SerializeField] LevelColor levelColor;
    [SerializeField] ColorData colorData;
    LineRenderer[] lineRenderers;
    Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        if (!colorChange) return;

        lineRenderers = GetComponentsInChildren<LineRenderer>();
        newColor = colorData.GetColor(levelColor);

        foreach(LineRenderer line in lineRenderers)
        {
            line.startColor = newColor;
            line.endColor = newColor;
        }
    }

}
