using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteWake : MonoBehaviour
{
    MinibossDroneController droneController;

    // Start is called before the first frame update
    void Start()
    {
        droneController = GetComponentInParent<MinibossDroneController>();
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
