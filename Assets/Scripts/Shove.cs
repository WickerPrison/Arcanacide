using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shove : MonoBehaviour
{
    [SerializeField] MeshRenderer sphereRenderer;
    bool isShoving = false;
    float minSize = 0.1f;
    float maxSize = 2;
    Vector3 growthSpeed = new Vector3(1, .5f, 1);

    // Start is called before the first frame update
    void Start()
    {
        sphereRenderer.enabled = false;
    }

    private void Update()
    {
        if (isShoving)
        {
            transform.localScale += growthSpeed * Time.deltaTime;
            if(transform.localScale.x >= maxSize)
            {
                isShoving = false;
                sphereRenderer.enabled = false;
            }
        }
    }

    public void ShoveVFX()
    {
        sphereRenderer.enabled = true;
        transform.localScale = new Vector3(minSize, minSize, minSize);
        isShoving = true;
    }
}
