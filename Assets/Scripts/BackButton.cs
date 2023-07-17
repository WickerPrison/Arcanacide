using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] MapData mapData;
    [SerializeField] GameObject button;
    [SerializeField] Image backTriangle;
    Vector3 initialScale;
    float transitionTime = 0.1f;
    float scaleMultiplier = 1.25f;

    private void Awake()
    {
        initialScale = button.transform.localScale;
        backTriangle.color = mapData.floorColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        backTriangle.color = Color.white;
        StartCoroutine(ScaleAnimation(button.transform.localScale, initialScale * scaleMultiplier));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        backTriangle.color = mapData.floorColor;
        StartCoroutine(ScaleAnimation(button.transform.localScale, initialScale));
    }

    IEnumerator ScaleAnimation(Vector3 currentScale, Vector3 targetScale)
    {
        float timer = transitionTime;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;

            button.transform.localScale = Vector3.Lerp(targetScale, currentScale, ratio);
            yield return new WaitForEndOfFrame();
        }
    }
}
