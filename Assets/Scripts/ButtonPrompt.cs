using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] string KBMprompt;
    [SerializeField] string GPprompt;
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(Gamepad.current == null)
        {
            text.text = KBMprompt.Replace("\\n", "\n");
        }
        else
        {
            text.text = GPprompt.Replace("\\n", "\n");
        }
    }
}
