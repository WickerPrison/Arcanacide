using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject shopWindowPrefab;
    GameObject shopWindow;
    Transform player;
    InputManager im;
    float playerDistance;
    float interactDistance = 2;

    [SerializeField] GameObject dialoguePrefab;
    DialogueScript dialogue;
    int tracker = 0;
    string dialogue1 = "Hello there! Good to see you're still kicking!";
    string dialogue2 = "I have some interesting Emblems if you're in the market for 'em.\nCare to take a look?";
    string dialogue3 = "Goodbye then. Stay safe friend.";

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => StartDialogue();
        im.controls.Dialogue.Next.performed += ctx => Talk();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

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

    void StartDialogue()
    {
        if(playerDistance <= interactDistance)
        {
            dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
            dialogue.SetImage("Patchwork Gary");
            dialogue.SetText(dialogue1);
            im.Dialogue();
        }
    }

    void Talk()
    {
        switch (tracker)
        {
            case 0:
                tracker += 1;
                dialogue.SetText(dialogue2);
                break;
            case 1:
                tracker += 1;
                OpenShop();
                Destroy(dialogue.gameObject);
                break;
            case 2:
                tracker = 0;
                im.Gameplay();
                Destroy(dialogue.gameObject);
                break;
        }
    }

    void OpenShop()
    {
        if(playerDistance <= interactDistance)
        {
            im.Menu();
            shopWindow = Instantiate(shopWindowPrefab);
        }
    }

    public void CloseShop()
    {
        Destroy(shopWindow);
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        dialogue.SetImage("Patchwork Gary");
        dialogue.SetText(dialogue3);
        tracker = 2;
        im.Dialogue();
    }
}
