using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisableForBuildModes : MonoBehaviour
{
    [SerializeField] BuildMode buildMode;
    [SerializeField] BuildModes[] disableModes;

    void Start()
    {
        if (disableModes.Contains(buildMode.buildMode))
        {
            gameObject.SetActive(false);
        }
    }
}
