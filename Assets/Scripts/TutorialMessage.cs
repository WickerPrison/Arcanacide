using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI continueMessage;
    string KBMcontinue = "Continue: E";
    string GPcontinue = "Continue: <sprite index= 0>";
    [SerializeField] TextMeshProUGUI tutorialMessage;
    [SerializeField] string KBMtutorial;
    [SerializeField] string GPtutorial;

    private void Update()
    {
        if(Gamepad.current == null)
        {
            continueMessage.text = KBMcontinue;
            tutorialMessage.text = KBMtutorial.Replace("\\n", "\n");
        }
        else
        {
            continueMessage.text = GPcontinue;
            tutorialMessage.text = GPtutorial.Replace("\\n", "\n"); ;
        }
    }
}
