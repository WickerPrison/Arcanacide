using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPassword : MonoBehaviour
{
    [SerializeField] int resetIndex;
    [SerializeField] GameObject message;
    [SerializeField] float interactDistance = 2;
    [SerializeField] ParticleSystem wayFaerie;
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] WayFaerieManager wayFaerieManager;
    Dialogue dialogue;
    Transform player;
    InputManager im;
    float playerDistance = 100;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        dialogue = GetComponent<Dialogue>();
        if(mapData.resetPasswords == null)
        {
            dialogue.conversationNum = 4;
        }
        else
        {
            dialogue.conversationNum = mapData.resetPasswords.Count;
        }

        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
    }

    void Start()
    {
        im.controls.Gameplay.Interact.performed += ctx => Investigate();
        if (mapData.resetPasswords != null && mapData.resetPasswords.Contains(resetIndex))
        {
            if (wayFaerie != null)
            {
                wayFaerie.Clear();
                wayFaerie.Stop();
            }
        }
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (mapData.resetPasswords != null && mapData.resetPasswords.Contains(resetIndex))
        {
            if (wayFaerie != null) wayFaerie.Stop();
            message.SetActive(false);
            return;
        }

        if (playerDistance <= interactDistance)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void Investigate()
    {
        if (playerDistance > interactDistance) return;

        if(mapData.resetPasswords == null)
        {
            if (wayFaerie != null) wayFaerie.Stop();
            dialogue.StartConversation();
        }
        else if (!mapData.resetPasswords.Contains(resetIndex))
        {
            mapData.resetPasswords.Add(resetIndex);
            if (wayFaerie != null) wayFaerie.Stop();
            if(mapData.resetPasswords.Count == 3)
            {
                dialogue.StartWithCallback(() =>
                {
                    playerData.hasWayfaerie = true;
                    wayFaerieManager.SendWayfaerie();
                });
            }
            else
            {
                dialogue.StartConversation();
            }
        }
    }
}
