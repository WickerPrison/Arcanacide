using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OptionSetUi : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image background;
    [SerializeField] Image leftButton;
    [SerializeField] Image rightButton;
    [SerializeField] TextMeshProUGUI[] optionTexts;
    float transitionTime = 0.2f;
    [System.NonSerialized] public bool selected = false;

    public void SelectOption(int choice)
    {
        StopAllCoroutines();
        StartCoroutine(SelectAnimation(choice));
    }

    public void ImmediateSelectOption(int choice)
    {
        StopAllCoroutines();
        for (int i = 0; i < optionTexts.Length; i++)
        {
            int diff = i - choice;
            optionTexts[i].rectTransform.localPosition = new Vector3(150 * diff, 0, 0);
        }
    }

    IEnumerator SelectAnimation(int choice)
    {
        Vector3[] startingPositions = optionTexts.Select(text => text.rectTransform.localPosition).ToArray();
        Vector3[] finalPositions = new Vector3[startingPositions.Length]; 
        for (int i = 0; i < optionTexts.Length; i++)
        {
            int diff = i - choice;
            finalPositions[i] = new Vector3(150 * diff, 0, 0);
        }
        float timer = 0f;
        while (timer < transitionTime)
        {
            timer += Time.unscaledDeltaTime;
            for(int i = 0; i < optionTexts.Length; i++)
            {
                optionTexts[i].rectTransform.localPosition = Vector3.Lerp(startingPositions[i], finalPositions[i], timer / transitionTime);
            }
            yield return null;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        background.color = Color.black;
        leftButton.color = Color.white;
        rightButton.color = Color.white;
        foreach(TextMeshProUGUI text in optionTexts)
        {
            text.color = Color.white;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
        background.color = Color.white;
        leftButton.color = Color.black;
        rightButton.color = Color.black;
        foreach (TextMeshProUGUI text in optionTexts)
        {
            text.color = Color.black;
        }
    }
}
