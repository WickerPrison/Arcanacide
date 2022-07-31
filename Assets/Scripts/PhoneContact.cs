using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneContact : MonoBehaviour
{
    public PhoneLibrary phoneLibrary;
    string contactName;
    [SerializeField] TextMeshProUGUI text;

    public void SetContactName(string name)
    {
        contactName = name;
        text.text = name;
    }

    public void StartConversation()
    {
        phoneLibrary.StartConversation(contactName);
    }
}
