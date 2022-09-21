using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    [SerializeField] RectTransform text;
    [SerializeField] RectTransform wrapper;
    public bool leftAligned = true;
    HorizontalLayoutGroup wrapperGroup;
    ContentSizeFitter fitter;
    RectTransform rectTransform;
    TextMeshProUGUI textText;
    float maxBoxWidth = 287;


    private void Start()
    {
        wrapperGroup = wrapper.gameObject.GetComponent<HorizontalLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        textText = text.GetComponent<TextMeshProUGUI>();
        fitter = text.gameObject.GetComponent<ContentSizeFitter>();

        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (!leftAligned)
        {
            wrapperGroup.childAlignment = TextAnchor.MiddleRight;
        }

        if (text.sizeDelta.x > maxBoxWidth)
        {
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            text.sizeDelta = new Vector2(maxBoxWidth, text.sizeDelta.y);
            textText.margin = new Vector4(textText.margin.x, textText.margin.y, 0, textText.margin.w);
        }

        rectTransform.sizeDelta = text.sizeDelta;
        wrapper.sizeDelta = new Vector2(wrapper.sizeDelta.x, text.sizeDelta.y);
    }
}
