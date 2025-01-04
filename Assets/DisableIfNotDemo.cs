using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfNotDemo : MonoBehaviour
{
    [SerializeField] BuildMode buildMode;

    // Start is called before the first frame update
    void Start()
    {
        if (buildMode.buildMode != BuildModes.DEMO) gameObject.SetActive(false);
    }
}
