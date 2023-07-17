using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfDemo : MonoBehaviour
{
    [SerializeField] DemoMode demoMode;

    // Start is called before the first frame update
    void Start()
    {
        if(demoMode.demoMode) gameObject.SetActive(false);
    }
}
