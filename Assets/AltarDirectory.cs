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
            case 2:
                return "FireHub1";
            case 3:
                return "FireHub2";
            default:
                return "Error";
        }
    }
}
