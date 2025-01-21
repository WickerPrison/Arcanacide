using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFloorColor : MonoBehaviour
{
    [SerializeField] MapData mapData;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.color = mapData.floorColor;
    }
}
