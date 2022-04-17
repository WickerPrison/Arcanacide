using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ContinueMessage : MonoBehaviour
{
    TextMeshProUGUI continueMessage;
    string KBMcontinue = "Continue: E";
    string GPcontinue = "Continue: <sprite index= 0>";

    private void Start()
    {
        continueMessage = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Gamepad.current == null)
        {
            continueMessage.text = KBMcontinue;
        }
        else
        {
            continueMessage.text = GPcontinue;
        }
    }
}
