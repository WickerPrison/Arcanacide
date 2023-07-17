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

    Vector3 initialScale;
    float scaleMultiplier = 1.07f;
    float pulseSpeed = 3;
    float pulseValue;

    private void Start()
    {
        continueMessage = gameObject.GetComponent<TextMeshProUGUI>();
        initialScale = transform.localScale;
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

        pulseValue = Mathf.Cos(Time.unscaledTime * pulseSpeed) * 0.5f + 0.5f;
        transform.localScale = Vector3.Lerp(initialScale * scaleMultiplier, initialScale, pulseValue);
    }
}
