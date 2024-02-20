using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorGlare : MonoBehaviour
{
    [SerializeField] RectTransform door;
    Image image;
    float closedPos = -8.888889f;
    float openPos = 2.7f;
    float distance;
    float endPos = 1207;
    float openAmount;

    private void Start()
    {
        image = GetComponent<Image>();
        distance = openPos - closedPos;
    }

    private void Update()
    {
        HideGlare();
    }

    void HideGlare()
    {
        openAmount = door.position.x / endPos;
        image.fillAmount = 1- openAmount;
    }
}
