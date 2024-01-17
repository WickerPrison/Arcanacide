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
    [SerializeField] Sprite mageImage;
    [SerializeField] Sprite patchworkGaryImage;
    [SerializeField] Sprite headOfITImage;
    [SerializeField] Sprite QuestionMarksImage;
    [SerializeField] Sprite fatManImage;
    [SerializeField] Sprite headOfHRImage;
    [SerializeField] Sprite whistleblowerImage;
    [SerializeField] Sprite freiImage;
    [SerializeField] Sprite headOfAccountingImage;
    [SerializeField] Sprite CEOImage;
    [SerializeField] Sprite AssistantImage;
    Dictionary<string, Sprite> imageDict;

    private void Awake()
    {
        SetupDictionary();
    }

    public void SetText(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public void SetImage(string characterName)
    {
        nameText.text = characterName + ":";
        speakerImage.sprite = imageDict[characterName];
    }

    void SetupDictionary()
    {
        imageDict = new Dictionary<string, Sprite>
        {
            {"Agent", playerImage },
            {"Agent Adams", playerImage },
            {"Ernie", janitorImage },
            {"Mage", mageImage },
            {"Secretary", mageImage },
            {"Patchwork Gary", patchworkGaryImage},
            {"Head of IT", headOfITImage },
            {"Fat Man", fatManImage },
            {"Catherine Pope", whistleblowerImage },
            {"Carol", headOfHRImage },
            {"Agent Frei", freiImage },
            {"Head of Accounting", headOfAccountingImage },
            {"CEO", CEOImage },
            {"Assistant", AssistantImage }
        };
    }
}
