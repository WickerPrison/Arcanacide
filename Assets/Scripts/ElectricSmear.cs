using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSmear : MonoBehaviour
{
    Smear smear;
    [SerializeField] List<Vector3> smearScales = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        smear = GetComponentInParent<Smear>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = smearScales[smear.facingDirection];
    }
}
