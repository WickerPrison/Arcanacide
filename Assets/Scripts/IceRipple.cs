using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRipple : MonoBehaviour
{
    [SerializeField] GameObject iceBoxPrefab;
    float startRadius = 0.5f;
    int numberOfBoxes = 50;
    float rippleSpeed = 10;
    float lifeTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        float rotateAngle = 360 / numberOfBoxes;
        for(int box = 0; box < numberOfBoxes; box++)
        {
            IceBox iceBox = Instantiate(iceBoxPrefab).GetComponent<IceBox>();
            iceBox.transform.position = transform.position + new Vector3(startRadius, 0, 0);
            iceBox.transform.RotateAround(transform.position, transform.up, rotateAngle * box);
            iceBox.rippleSpeed = rippleSpeed;
            iceBox.lifeTime = lifeTime;
            iceBox.origin = transform.position;
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
