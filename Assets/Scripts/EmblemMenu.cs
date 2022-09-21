using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EmblemMenu : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject equipEmblemPrefab;
    [SerializeField] Transform canvas;
    [SerializeField] GameObject noEmblemsMessage;
    [SerializeField] Transform content;
    [SerializeField] ScrollRect scrollRect;
    public GameObject leaveButton;
    Button leaveButtonButton;
    public RestMenuButtons restMenuScript;
    public List<GameObject> buttons = new List<GameObject>();
    public int altarNumber;
    public Transform spawnPoint;
    PlayerControls controls;
    SoundManager sm;
    float scrollDir;
    float scrollSpeed = .1f;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => OpenRestMenu();
        controls.Menu.Scroll.performed += ctx => scrollDir = ctx.ReadValue<Vector2>().y;
        controls.Menu.Scroll.canceled += ctx => scrollDir = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        SpawnEmblems();
        if(playerData.emblems.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0]);
            leaveButtonButton = leaveButton.GetComponent<Button>();
            Navigation nav = leaveButtonButton.navigation;
            nav.selectOnUp = buttons[buttons.Count - 1].GetComponent<Button>();
            nav.selectOnDown = buttons[0].GetComponent<Button>();
            leaveButtonButton.navigation = nav;
            noEmblemsMessage.SetActive(false);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(leaveButton);
        }
    }

    private void Update()
    {
        if(playerData.emblems.Count > 0)
        {
            if(Gamepad.current == null)
            {
                MouseScrollPosition();
            }
            else
            {
                ControllerScrollPosition();
            }
        }
    }

    public void OpenRestMenu()
    {
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restMenuScript.firstButton);
        restMenuScript.controls.Enable();
        Destroy(gameObject);
    }

    void SpawnEmblems()
    {
        for(int i = 0; i < playerData.emblems.Count; i++)
        {
            GameObject equipEmblem = Instantiate(equipEmblemPrefab);
            buttons.Add(equipEmblem);
            equipEmblem.transform.SetParent(content);
            EquipEmblem equipEmblemScript = equipEmblem.GetComponent<EquipEmblem>();
            equipEmblemScript.emblemName = playerData.emblems[i];
            equipEmblemScript.emblemMenu = this;
        }
    }

    void ControllerScrollPosition()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;
        if (!buttons.Contains(currentButton))
        {
            return;
        }

        int index = buttons.IndexOf(currentButton);
        float position = (float)index / ((float)buttons.Count - 1);
        position = 1 - position;

        float scrollDiff = position - scrollRect.verticalNormalizedPosition;
        scrollDir = scrollDiff / Mathf.Abs(scrollDiff);
        float scrollDistance = scrollDiff * 2f * Time.deltaTime + scrollDir * .4f * Time.deltaTime;
        if (Mathf.Abs(scrollDiff) < scrollDistance)
        {
            scrollRect.verticalNormalizedPosition = position;
        }
        else
        {
            scrollRect.verticalNormalizedPosition += scrollDistance;
        }
    }

    void MouseScrollPosition()
    {
        scrollDir /= 120;
        scrollRect.verticalNormalizedPosition += scrollDir * scrollSpeed;
        if(scrollRect.verticalNormalizedPosition > 1)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
        else if(scrollRect.verticalNormalizedPosition < 0)
        {
            scrollRect.verticalNormalizedPosition = 0;
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
