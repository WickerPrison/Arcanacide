using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PhoneMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    TextMeshProUGUI buttonText;
    Vector3 initialScale;
    float transitionTime = 0.1f;
    [SerializeField] float scaleMultiplier = 1.15f;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        initialScale = transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(buttonText != null) buttonText.color = Color.white;
        StartCoroutine(ScaleAnimation(transform.localScale, initialScale * scaleMultiplier));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (buttonText != null) buttonText.color = Color.black;
        StartCoroutine(ScaleAnimation(transform.localScale, initialScale));
    }

    IEnumerator ScaleAnimation(Vector3 currentScale, Vector3 targetScale)
    {
        float timer = transitionTime;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;

            transform.localScale = Vector3.Lerp(targetScale, currentScale, ratio);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ButtonPressAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(ButtonPress(transform.localScale));
    }

    IEnumerator ButtonPress(Vector3 currentScale)
    {
        float timer = transitionTime;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;

            transform.localScale = Vector3.Lerp(initialScale * 0.8f, currentScale, ratio);
            yield return new WaitForEndOfFrame();
        }

        timer = transitionTime;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;

            transform.localScale = Vector3.Lerp(initialScale * scaleMultiplier, initialScale * 0.8f, ratio);
            yield return new WaitForEndOfFrame();
        }
    }
}
