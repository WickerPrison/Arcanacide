using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneMenu : MonoBehaviour
{
    [SerializeField] PhoneData phoneData;
    [SerializeField] List<PhoneContact> phoneContacts;
    [SerializeField] GameObject contactObjects;
    [SerializeField] GameObject lineDeadObject;
    public TextingLibrary phoneLibrary;
    List<string> activeContacts;

    void Start()
    {
        activeContacts = phoneData.GetNewMessages();
        if(activeContacts.Count > 0)
        {
            lineDeadObject.SetActive(false);
            contactObjects.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(phoneContacts[0].gameObject);
        }
        else
        {
            lineDeadObject.SetActive(true);
            contactObjects.SetActive(false);
        }

        for(int contact = 0; contact < phoneContacts.Count; contact++)
        {
            if(activeContacts.Count > 0)
            {
                phoneContacts[contact].gameObject.SetActive(true);
                phoneContacts[contact].SetContactName(activeContacts[0]);
                activeContacts.Remove(activeContacts[0]);
                phoneContacts[contact].phoneLibrary = phoneLibrary;
            }
            else
            {
                phoneContacts[contact].gameObject.SetActive(false);
            }
        }
    }
}
