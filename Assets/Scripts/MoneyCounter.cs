using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = playerData.money.ToString();
    }
}
