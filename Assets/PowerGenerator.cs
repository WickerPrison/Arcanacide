using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : MonoBehaviour
{
    [SerializeField] Transform teslaBall1;
    [SerializeField] Transform teslaBall2;
    Bolts boltsScript;
    Vector3 away = new Vector3 (100, 100, 100);

    private void Awake()
    {
        boltsScript = GetComponentInChildren<Bolts>();
        boltsScript.SetPositions(teslaBall1.position, teslaBall2.position);
    }

    public void PowerOff()
    {
        boltsScript.SetPositions(away,away);
    }
}
