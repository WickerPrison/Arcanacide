using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossStalagmites : MonoBehaviour
{
    public float stalagmiteTimer;

    StalagmiteAttack[] stalagmites;
    [SerializeField] bool colorChange;
    [SerializeField] Color oldColor;
    [SerializeField] LevelColor newColor;
    [SerializeField] ColorData colorData;

    private void Start()
    {
        if (!colorChange) return;
        stalagmites = GetComponentsInChildren<StalagmiteAttack>();
        foreach (StalagmiteAttack stalagmite in stalagmites)
        {
            stalagmite.ColorChange(oldColor, colorData.GetColor(newColor));
        }
    }
}
