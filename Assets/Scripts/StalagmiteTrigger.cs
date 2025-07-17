using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteTrigger : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, 0.5f);
        transform.LookAt(endPoint);
        boxCollider.size = new Vector3(0.1f, 5f, Vector3.Distance(startPoint.position, endPoint.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        MinibossStalagmiteAttack stalagmite = other.GetComponent<MinibossStalagmiteAttack>();
        if(stalagmite != null)
        {
            stalagmite.TriggerStalagmite();
        }
    }
}
