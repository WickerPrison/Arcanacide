using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaryACtoggle : MonoBehaviour
{
    [SerializeField] MapData mapData;
    Shop garyScript;

    // Start is called before the first frame update
    void Start()
    {
        garyScript = GetComponent<Shop>();
        garyScript.useAlternate = !mapData.ACOn;
    }
}
