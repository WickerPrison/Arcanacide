using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBackground : MonoBehaviour
{
    [SerializeField] float borderSize;
    [SerializeField] RectTransform blackBackground;
    [SerializeField] RectTransform whiteBackground;

    private void Start()
    {
        UpdateBackgroundSize();
    }

    private void Update()
    {
        UpdateBackgroundSize();   
    }

    void UpdateBackgroundSize()
    {
        whiteBackground.sizeDelta = blackBackground.sizeDelta + new Vector2(borderSize, borderSize);
    }

    private void OnDrawGizmosSelected()
    {
        UpdateBackgroundSize();
    }
}
