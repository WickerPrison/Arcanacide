using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : MonoBehaviour
{
    [SerializeField] Transform teslaBall1;
    [SerializeField] Transform teslaBall2;
    Bolts boltsScript;
    Vector3 away = new Vector3(100, 100, 100);

    // Start is called before the first frame update
    void Start()
    {
        boltsScript = GetComponentInChildren<Bolts>();
        boltsScript.SetPositions(teslaBall1.position, teslaBall2.position);
    }

    public void TurnOff()
    {
        boltsScript.SetPositions(away, away);
    }
}
