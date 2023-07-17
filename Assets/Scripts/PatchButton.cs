using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class PatchButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] TextMeshProUGUI patchName;
    [SerializeField] Image patch;
    [SerializeField] Image stitches;
    [SerializeField] GameObject patchButton;
    [SerializeField] GameObject description;
    [SerializeField] RectTransform descriptionText;
    [SerializeField] RectTransform patchParent;
    [System.NonSerialized] public RectTransform contentRect;
    Vector3 initialScale;
    float transitionTime = 0.2f;
    float scaleMultiplier = 1.1f;

    float descriptionTransition = 0.2f;

    Vector2 initialRectSize = new Vector2(100, 100);

    private void Awake()
    {
        initialScale = patchButton.transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        patchName.color = Color.white;
        patch.color = Color.black;
        stitches.color = Color.white;

        StopAllCoroutines();
        StartCoroutine(PatchScaleAnimation(patchButton.transform.localScale, initialScale * scaleMultiplier));
        StartCoroutine(DescriptionScaleAnimation(descriptionText.localScale.y, 3));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        patchName.color = Color.black;
        patch.color = Color.white;
        stitches.color = Color.black;

        StopAllCoroutines();
        StartCoroutine(PatchScaleAnimation(patchButton.transform.localScale, initialScale));
        StartCoroutine(DescriptionScaleAnimation(descriptionText.localScale.y, 0));
    }

    IEnumerator PatchScaleAnimation(Vector3 currentScale, Vector3 targetScale)
    {
        float timer = transitionTime;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;

            patchButton.transform.localScale = Vector3.Lerp(targetScale, currentScale, ratio);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DescriptionScaleAnimation(float currentScale, float targetScale)
    {
        float timer = descriptionTransition;

        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;

            float ratio = timer / descriptionTransition;

            float yScale = Mathf.Lerp(targetScale, currentScale, ratio);
            descriptionText.localScale = new Vector3(descriptionText.localScale.x, yScale, descriptionText.localScale.z);
            patchParent.sizeDelta = new Vector2(100, 100 + descriptionText.sizeDelta.y * descriptionText.localScale.y);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

            yield return new WaitForEndOfFrame();
        }
    }
}
