using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] PhoneData phoneData;
    [SerializeField] GameObject message;
    [SerializeField] GameObject phoneMenu;
    PhoneMenu phoneMenuScript;
    PhoneLibrary phoneLibrary;
    Transform player;
    InputManager im;
    float playerDistance;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => UsePhone();
        im.controls.Dialogue.Next.performed += ctx => phoneLibrary.NextDialogue();
        im.controls.Menu.Back.performed += ctx => HangUp();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        phoneLibrary = GetComponent<PhoneLibrary>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void UsePhone()
    {
        if (playerDistance <= interactDistance)
        {
            phoneMenuScript = Instantiate(phoneMenu).GetComponent<PhoneMenu>();
            phoneMenuScript.phoneLibrary = phoneLibrary;
            im.Menu();
        }
    }

    void HangUp()
    {
        Destroy(phoneMenuScript.gameObject);
        im.Gameplay();
    }

    public void StartDialogue()
    {
        Destroy(phoneMenuScript.gameObject);
        im.Dialogue();
    }

    public void EndDialogue()
    {
        im.Gameplay();
    }
}
