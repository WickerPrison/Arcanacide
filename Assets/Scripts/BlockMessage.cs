using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMessage : MonoBehaviour
{
    [SerializeField] GameObject message1;
    [SerializeField] GameObject message2;
    int counter;
    InputManager im;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Tutorial.Select.performed += ctx => NextMessage();
        counter = 1;
        message2.SetActive(false);
    }

    void NextMessage()
    {
        if (counter == 1)
        {
            counter += 1;
            message1.SetActive(false);
            message2.SetActive(true);
        }
        else if (counter == 2)
        {
            im.Gameplay();
            Destroy(gameObject);
        }
    }
}
