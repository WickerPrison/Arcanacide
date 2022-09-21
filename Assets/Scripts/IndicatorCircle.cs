using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorCircle : MonoBehaviour
{
    [SerializeField] Transform indicatorCircle;
    public float startScale;
    public float finalScale;
    public float growthTime;
    public float deathTime;
    public bool destroyParent;
    float growthRate;
    Vector3 growthVector;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        indicatorCircle.localScale = new Vector3(startScale, startScale, startScale);
        growthRate = (finalScale - startScale) / growthTime;
        growthVector = new Vector3(growthRate * Time.fixedDeltaTime, growthRate * Time.fixedDeltaTime, growthRate * Time.fixedDeltaTime);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= deathTime)
        {
            if (destroyParent)
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (timer < growthTime)
        {
            indicatorCircle.localScale += growthVector;
        }
    }
}
