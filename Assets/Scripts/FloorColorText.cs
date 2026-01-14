using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloorColorText : MonoBehaviour
{
    [SerializeField] MapData mapData;

    // Start is called before the first frame update
    void Start()
    {
        if(mapData.floor == 2)
        {
            GetComponent<TextMeshProUGUI>().color = Color.black;
        }
    }
}
