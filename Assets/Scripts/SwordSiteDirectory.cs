using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSiteDirectory : MonoBehaviour
{
    
    public string GetSceneName(int swordSiteNumber)
    {
        switch (swordSiteNumber)
        {
            case 1:
                return "Tutorial1";
            case 2:
                return "FireHub1";
            case 3:
                return "FireHub2";
            case 4: 
                return "ElectricHub";
            default:
                return "Error";
        }
    }
}
