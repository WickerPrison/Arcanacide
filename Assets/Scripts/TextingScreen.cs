using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TextingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contactNameText;
    [SerializeField] GameObject textBoxPrefab;
    [SerializeField] DialogueData phoneData;
    [SerializeField] Transform content;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject newLinePrefab;
    [SerializeField] GameObject continueMessage;
    [SerializeField] Button leaveButton;
    [SerializeField] ScrollRect scrollRect;
    AudioSource audioSource;
    public TextingMenu textingMenu;
    TextingLibrary textingLibrary;
    public string contactName;
    PlayerControls controls;
    SoundManager sm;
    public List<int> previousConversations;
    public List<int> conversationQueue;
    List<List<string>> conversations;
    bool newTexts = false;
    List<string> newConversation;
    bool spawnLine = false;
    float scrollDir;
    float scrollSpeed = .2f;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => ReturnToContacts();
        controls.Menu.Select.performed += ctx => NextText();
        controls.Menu.ControllerDirection.performed += ctx => scrollDir = ctx.ReadValue<Vector2>().y;
        controls.Menu.ControllerDirection.canceled += ctx => scrollDir = 0;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        EventSystem.current.SetSelectedGameObject(null);
        textingLibrary = GetComponent<TextingLibrary>();
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        contactNameText.text = contactName;
        conversations = textingLibrary.GetConversations(contactName, this);
        SpawnOldTexts();
        if (conversationQueue.Count > 0)
        {
            NewTexts();
        }
        else StartCoroutine(ReactivateBackButton());
    }

    private void Update()
    {
        if(Gamepad.current == null)
        {
            scrollRect.vertical = true;
        }
        else
        {
            scrollRect.vertical = false;
            ControllerScrollPosition();
        }
    }

    void ControllerScrollPosition()
    {
        scrollRect.verticalNormalizedPosition += scrollDir * scrollSpeed;
        if (scrollRect.verticalNormalizedPosition > 1)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
        else if (scrollRect.verticalNormalizedPosition < 0)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    void SpawnOldTexts()
    {
        if(previousConversations.Count == 0)
        {
            return;
        }

        for (int i = 0; i < previousConversations.Count; i++)
        {
            Transform line = Instantiate(linePrefab).transform;
            line.SetParent(content);
            int conversationIndex = previousConversations[i];
            List<string> currentConversation = conversations[conversationIndex];
            for(int n = 0; n < currentConversation.Count; n++)
            {
                SpawnTextBox(currentConversation, n);
            }
        }
    }

    void NewTexts()
    {
        continueMessage.SetActive(true);
        newTexts = true;
        int conversationIndex = conversationQueue[0];
        newConversation = conversations[conversationIndex];
        string[] currentLine = newConversation[0].Split('|');

        if (currentLine[0] != "Agent")
        {
            Transform newLine = Instantiate(newLinePrefab).transform;
            newLine.SetParent(content);
            NextText();
        }
        else
        {
            spawnLine = true;
        }
    }

    void NextText()
    {
        if (!newTexts)
        {
            return;
        }

        if (spawnLine)
        {
            Transform newLine = Instantiate(newLinePrefab).transform;
            newLine.SetParent(content);
            spawnLine = false;
        }

        SpawnTextBox(newConversation, 0);
        newConversation.RemoveAt(0);

        if (newConversation.Count == 0)
        {
            previousConversations.Add(conversationQueue[0]);
            textingLibrary.AddToQueue(contactName, conversationQueue[0]);
            conversationQueue.RemoveAt(0);
            continueMessage.SetActive(false);
            newTexts = false;
            StartCoroutine(ReactivateBackButton());
        }
    }

    IEnumerator ReactivateBackButton()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(leaveButton.gameObject);
    }

    void SpawnTextBox(List<string> currentConversation, int lineIndex)
    {
        audioSource.Play();
        GameObject textBox = Instantiate(textBoxPrefab);
        textBox.transform.SetParent(content);
        TextBox textBoxScript = textBox.GetComponentInChildren<TextBox>();
        TextMeshProUGUI textBoxText = textBox.GetComponentInChildren<TextMeshProUGUI>();
        string[] currentLine = currentConversation[lineIndex].Split('|');
        textBoxText.text = currentLine[1];
        if (currentLine[0] == "Agent")
        {
            textBoxScript.leftAligned = false;
        }
        else
        {
            textBoxScript.leftAligned = true;
        }
    }

    public void ReturnToContacts()
    {
        if (newTexts)
        {
            return;
        }
        
        sm.ButtonSound();
        textingMenu.SpawnContacts();
        textingMenu.controls.Enable();
        Destroy(gameObject);
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
