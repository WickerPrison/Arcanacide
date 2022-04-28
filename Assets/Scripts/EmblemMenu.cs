using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmblemMenu : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject equipEmblemPrefab;
    [SerializeField] Transform canvas;
    [SerializeField] GameObject noEmblemsMessage;
    public GameObject leaveButton;
    Button leaveButtonButton;
    public RestMenuButtons restMenuScript;
    public List<GameObject> buttons = new List<GameObject>();
    Vector3 firstEmblemPosition = new Vector3(350, 850, 0);
    public int altarNumber;
    public Transform spawnPoint;
    PlayerControls controls;
    SoundManager sm;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => OpenRestMenu();
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
            nav.selectOnDown = buttons[0].GetComponent<Button>();
            nav.selectOnLeft = buttons[0].GetComponent<Button>();
            leaveButtonButton.navigation = nav;
            noEmblemsMessage.SetActive(false);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(leaveButton);
        }
    }

    public void OpenRestMenu()
    {
        /*
        restMenu = Instantiate(restMenuPrefab);
        RestMenuButtons restMenuScript = restMenu.GetComponent<RestMenuButtons>();
        restMenuScript.altarNumber = altarNumber;
        restMenuScript.spawnPoint = spawnPoint;
        */
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
            equipEmblem.transform.SetParent(canvas.transform);
            Vector3 offset = new Vector3(0, i * 100, 0);
            RectTransform equipEmblemRect = equipEmblem.GetComponent<RectTransform>();
            equipEmblemRect.position = firstEmblemPosition - offset;
            EquipEmblem equipEmblemScript = equipEmblem.GetComponent<EquipEmblem>();
            equipEmblemScript.emblemName = playerData.emblems[i];
            equipEmblemScript.emblemMenu = this;
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
