using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantBeam : MonoBehaviour
{
    private void Start()
    {
        float xPos = Random.Range(-15, 15);
        float zPos = Random.Range(-15, 15);
        transform.position = new Vector3(xPos, 0, zPos);
    }
}
