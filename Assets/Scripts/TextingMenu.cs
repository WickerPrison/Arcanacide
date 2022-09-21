using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextingMenu : MonoBehaviour
{
    [SerializeField] PhoneData phoneData;
    [SerializeField] GameObject contactPrefab;
    [SerializeField] Transform content;
    public GameObject leaveButton;
    Button leaveButtonButton;
    public RestMenuButtons restMenuScript;
    List<string> contacts;
    List<string> newMessages;
    public PlayerControls controls;
    SoundManager sm;
    public GameObject firstButton;
    public List<GameObject> contactButtons = new List<GameObject>();

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => OpenRestMenu();
    }

    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        leaveButtonButton = leaveButton.GetComponent<Button>();
        SpawnContacts();
    }

    public void SpawnContacts()
    {
        ClearContent();
        phoneData.GetContacts(out contacts, out newMessages);
        for (int i = 0; i < contacts.Count; i++)
        {
            GameObject contact = Instantiate(contactPrefab);
            contact.transform.SetParent(content);
            contactButtons.Add(contact);
            ContactButton button = contact.GetComponent<ContactButton>();
            button.contactName = contacts[i];
            button.textingMenu = this;
            button.newMessage = newMessages.Contains(contacts[i]);
            button.listIndex = i;
            if(i == 0)
            {
                firstButton = contact;
            }
        }

        Navigation nav = leaveButtonButton.navigation;
        nav.selectOnUp = contactButtons[contactButtons.Count - 1].GetComponent<Button>();
        nav.selectOnDown = contactButtons[0].GetComponent<Button>();
        leaveButtonButton.navigation = nav;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void OpenRestMenu()
    {
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restMenuScript.firstButton);
        restMenuScript.controls.Enable();
        Destroy(gameObject);
    }

    void ClearContent()
    {
        contactButtons.Clear();
        foreach(Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
