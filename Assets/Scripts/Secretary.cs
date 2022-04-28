using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secretary : MonoBehaviour
{
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] MapData mapData;
    InputManager im;
    SoundManager sm;
    DialogueScript dialogue;
    int tracker = 0;

    string dialogue1 = "If you want to see the Boss you'll have to file a support ticket first. Let me check and see if there's one in the system.";
    string dialogue2 = "Hmmm....\nJust give me one second.";
    string dialogue3 = "It seems you have not filed a support ticket. I cannot let you see the Boss until you do so.";
    string dialogue4 = "Ah, there it is.\nThe Boss will see you now.";


    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        sm = im.gameObject.GetComponent<SoundManager>();
        im.controls.Dialogue.Next.performed += ctx => Talk();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!mapData.secretaryConvo)
        {
            dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
            dialogue.SetImage("Secretary");
            dialogue.SetText(dialogue1);
            im.Dialogue();
        }
    }

    void Talk()
    {
        sm.ButtonSound();
        switch (tracker)
        {
            case 0:
                tracker += 1;
                dialogue.SetText(dialogue2);
                break;
            case 1:
                tracker += 1;
                if (mapData.ticketFiled)
                {
                    dialogue.SetText(dialogue4);
                    mapData.secretaryConvo = true;
                }
                else
                {
                    dialogue.SetText(dialogue3);
                }
                break;
            case 2:
                tracker = 0;
                im.Gameplay();
                Destroy(dialogue.gameObject);
                break;
        }
    }
}
