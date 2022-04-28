using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapMenu : MonoBehaviour
{
    [SerializeField] RectTransform playerFace;
    [SerializeField] GameObject backButton;
    [SerializeField] List<GameObject> mapRooms;
    [SerializeField] MapData mapData;
    PlayerControls controls;
    public Vector3 playerFacePosition;
    public RestMenuButtons restMenuScript;
    SoundManager sm;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => OpenRestMenu();
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        UpdateMap();
        playerFace.localPosition = playerFacePosition;
    }

    public void OpenRestMenu()
    {
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restMenuScript.firstButton);
        restMenuScript.controls.Enable();
        Destroy(gameObject);
    }

    void UpdateMap()
    {
        ClearMap();
        if(mapData.visitedRooms.Count == 1 && mapData.visitedRooms.Contains(0))
        {
            mapRooms[0].SetActive(true);
        }
        else
        {
            
            foreach(int roomNumber in mapData.visitedRooms)
            {
                mapRooms[roomNumber].SetActive(true);
            }
            mapRooms[0].SetActive(false);
        }
    }

    void ClearMap()
    {
        foreach(GameObject mapPortion in mapRooms)
        {
            mapPortion.SetActive(false);
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
