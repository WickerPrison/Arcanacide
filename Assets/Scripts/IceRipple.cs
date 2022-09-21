using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRipple : MonoBehaviour
{
    [SerializeField] GameObject iceBoxPrefab;
    float startRadius = 2;
    int numberOfBoxes = 35;
    float rippleSpeed = 5;
    float lifeTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        float rotateAngle = 360 / numberOfBoxes;
        for(int box = 0; box < numberOfBoxes; box++)
        {
            RippleBox iceBox = Instantiate(iceBoxPrefab).GetComponent<RippleBox>();
            iceBox.transform.position = transform.position + new Vector3(startRadius, 0, 0);
            iceBox.transform.RotateAround(transform.position, transform.up, rotateAngle * box);
            iceBox.rippleSpeed = rippleSpeed;
            iceBox.lifeTime = lifeTime;
            iceBox.direction = Vector3.Normalize(iceBox.transform.position - transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
