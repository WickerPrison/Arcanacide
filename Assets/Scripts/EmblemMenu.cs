using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmblemMenu : MonoBehaviour
{
    [SerializeField] GameObject restMenuPrefab;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject equipEmblemPrefab;
    [SerializeField] Transform canvas;
    [SerializeField] GameObject noEmblemsMessage;
    Vector3 firstEmblemPosition = new Vector3(350, 850, 0);
    GameObject restMenu;
    public int altarNumber;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEmblems();
        if(playerData.emblems.Count > 0)
        {
            noEmblemsMessage.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenRestMenu();
        }
    }

    public void OpenRestMenu()
    {
        restMenu = Instantiate(restMenuPrefab);
        RestMenuButtons restMenuScript = restMenu.GetComponent<RestMenuButtons>();
        restMenuScript.altarNumber = altarNumber;
        restMenuScript.spawnPoint = spawnPoint;
        Destroy(gameObject);
    }

    void SpawnEmblems()
    {
        for(int i = 0; i < playerData.emblems.Count; i++)
        {
            GameObject equipEmblem = Instantiate(equipEmblemPrefab);
            equipEmblem.transform.SetParent(canvas.transform);
            Vector3 offset = new Vector3(0, i * 100, 0);
            RectTransform equipEmblemRect = equipEmblem.GetComponent<RectTransform>();
            equipEmblemRect.position = firstEmblemPosition - offset;
            EquipEmblem equipEmblemScript = equipEmblem.GetComponent<EquipEmblem>();
            equipEmblemScript.emblemName = playerData.emblems[i];
        }
    }
}
