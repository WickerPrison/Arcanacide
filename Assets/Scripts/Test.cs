using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector3 minSize;
    Vector3 maxSize;
    float value;
    float sizeFloat;
    bool growing = false;
    [SerializeField] float growthRate = 1;

    // Start is called before the first frame update
    void Start()
    {
        minSize = transform.localScale * 0.9f;
        maxSize = transform.localScale;
        value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (growing)
        {
            value -= Time.deltaTime * growthRate;
            if(value <= 0)
            {
                growing = false;
            }
        }
        else
        {
            value += Time.deltaTime * growthRate;
            if(value >= 1)
            {
                growing = true;
            }
        }

        sizeFloat = Mathf.SmoothStep(minSize.x, maxSize.x, value);
        transform.localScale = Vector3.one * sizeFloat;
    }
}
