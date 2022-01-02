using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject shopWindowPrefab;
    GameObject shopWindow;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2)
        {
            message.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenShop();
            }
        }
        else
        {
            message.SetActive(false);
            if(shopWindow != null)
            {
                Destroy(shopWindow);
            }
        }
    }

    void OpenShop()
    {
        shopWindow = Instantiate(shopWindowPrefab);
    }
}
