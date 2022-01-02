using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMessage : MonoBehaviour
{
    [SerializeField] GameObject message1;
    [SerializeField] GameObject message2;
    int counter;

    private void Start()
    {
        counter = 1;
        message2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(counter == 1)
            {
                counter += 1;
                message1.SetActive(false);
                message2.SetActive(true);
            }
            else if(counter == 2)
            {
                Destroy(gameObject);
            }
        }
    }
}
