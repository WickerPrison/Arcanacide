using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossBeamCrystal : MonoBehaviour
{
    bool grow = false;
    bool shrink = false;
    [System.NonSerialized] public bool spawnedIn = false;
    float growthTime = 1f;
    float growSpeed = 0.3f;
    float currentSize;


    private void Update()
    {
        if (grow)
        {
            Growing();
        }
        else if (shrink)
        {
            Shrinking();
        }
    }

    public void SpawnIn()
    {
        grow = true;
        shrink = false;
    }

    public void SpawnOut()
    {
        grow = false;
        shrink = true;
    }

    void Growing()
    {
        transform.localScale += Time.deltaTime * growSpeed * Vector3.one;
        currentSize += Time.deltaTime / growthTime;
        if(currentSize >= 1)
        {
            transform.localScale = growSpeed * Vector3.one;
            currentSize = 1;
            grow = false;
            spawnedIn = true;
        }
    }

    void Shrinking()
    {
        spawnedIn = false;
        transform.localScale -= Time.deltaTime * growSpeed * Vector3.one;
        currentSize -= Time.deltaTime / growthTime;
        if(currentSize <= 0)
        {
            transform.localScale = Vector3.zero;
            currentSize = 0;
            shrink = false;
        }
    }
}
