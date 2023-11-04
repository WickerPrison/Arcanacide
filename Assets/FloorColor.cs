using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorColor : MonoBehaviour
{
    [SerializeField] MapData mapData;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = mapData.floorColor;
    }
}
