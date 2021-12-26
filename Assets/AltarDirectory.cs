using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarDirectory : MonoBehaviour
{
    
    public string GetSceneName(int altarNumber)
    {
        switch (altarNumber)
        {
            case 1:
                return "Tutorial1";
            default:
                return "Error";
        }
    }
}
