using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] DialogueData phoneData;
    [SerializeField] GameObject message;
    [SerializeField] GameObject phoneMenu;
    PhoneMenu phoneMenuScript;
    TextingLibrary phoneLibrary;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;
    bool phoneInUse = false;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => UsePhone();
        //im.controls.Dialogue.Next.performed += ctx => phoneLibrary.NextDialogue();
        im.controls.Menu.Back.performed += ctx => HangUp();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        phoneLibrary = GetComponent<TextingLibrary>();
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
            phoneInUse = true;
            im.Menu();
        }
    }

    void HangUp()
    {
        if(phoneInUse)
        {
            phoneInUse = false;
            Destroy(phoneMenuScript.gameObject);
            im.Gameplay();
        }
    }

    public void StartDialogue()
    {
        phoneInUse = false;
        Destroy(phoneMenuScript.gameObject);
        im.Dialogue();
    }

    public void EndDialogue()
    {
        im.Gameplay();
    }
}
