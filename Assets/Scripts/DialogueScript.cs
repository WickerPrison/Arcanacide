using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image speakerImage;
    [SerializeField] Sprite playerImage;
    [SerializeField] Sprite janitorImage;
    [SerializeField] Sprite secretaryImage;
    [SerializeField] Sprite patchworkGaryImage;
    [SerializeField] Sprite headOfITImage;
    [SerializeField] Sprite ODPARCHMENTImage;
    [SerializeField] Sprite ODTRENCHImage;
    [SerializeField] Sprite QuestionMarksImage;

    public void SetText(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public void SetImage(string characterName)
    {
        nameText.text = characterName + ":";
        switch (characterName)
        {
            case "Player":
                speakerImage.sprite = playerImage;
                break;
            case "Ernie":
                speakerImage.sprite = janitorImage;
                break;
            case "Secretary":
                speakerImage.sprite = secretaryImage;
                break;
            case "Patchwork Gary":
                speakerImage.sprite = patchworkGaryImage;
                break;
            case "Head of IT":
                speakerImage.sprite = headOfITImage;
                break;
            case "ODPARCHMENT":
                speakerImage.sprite = ODPARCHMENTImage;
                break;
            case "ODTRENCH":
                speakerImage.sprite = ODTRENCHImage;
                break;
            case "??????":
                speakerImage.sprite = QuestionMarksImage;
                break;
        }
    }
}
