using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskToyBall : MonoBehaviour
{
    [SerializeField] Transform[] anchorPoints;
    LineRenderer[] lineRenderers;
    Vector3[] anchorPositions;

    private void Start()
    {
        lineRenderers = GetComponentsInChildren<LineRenderer>();
        anchorPositions = new Vector3[] { anchorPoints[0].position, anchorPoints[1].position };
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].SetPosition(0, transform.position);
            lineRenderers[i].SetPosition(1, anchorPositions[i]);
        }
    }
}
