using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CrystalMotion : MonoBehaviour
{
    Transform movePoint;
    Vector3 startingPoint;
    Vector3 newPosition;
    float cap = 0.3f;
    float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        movePoint = GetComponentsInChildren<Transform>()[1];
        movePoint.parent = transform.parent;
        startingPoint = transform.localPosition;
        SetNewPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = movePoint.localPosition -transform.localPosition;
        transform.localPosition += direction.normalized * speed * Time.fixedDeltaTime;

        if(Vector3.Distance(transform.localPosition, movePoint.localPosition) < 0.1)
        {
            SetNewPosition();
        }
    }

    void SetNewPosition()
    {
        newPosition = startingPoint + new Vector3(Random.Range(-cap, cap), Random.Range(-cap, cap), 0);
        movePoint.localPosition = newPosition;
    }
}
