using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] string description;
    [SerializeField] Image square;
    [SerializeField] Image triangle;
    [SerializeField] Transform parentTransform;
    [SerializeField] TextMeshProUGUI attributeName;
    [SerializeField] TextMeshProUGUI attributeValue;
    [SerializeField] Image background;

    LevelUpMenu menu;

    Vector3 initialScale;
    float transitionTime = 0.1f;
    float scaleMultiplier = 1.2f;

    float pressInTime = 0.1f;
    float unPressTime = 0.2f;
    float pressInMinscale = 0.9f;

    Vector3 centerPosition;
    Vector3 leftPosition;
    Vector3 rightPosition;
    float vibrateAmp = 5;
    float vibrateDuration = 0.3f;
    float vibrateFreq = 40;

    private void Awake()
    {
        initialScale = transform.localScale;
        menu = GetComponentInParent<LevelUpMenu>();
    }

    private void Start()
    {
        centerPosition = transform.localPosition;
        leftPosition = centerPosition - Vector3.right * vibrateAmp;
        rightPosition = centerPosition + Vector3.right * vibrateAmp;
    }

    public void OnSelect(BaseEventData eventData)
    {
        square.color = Color.black;
        triangle.color = Color.white;
        descriptionText.text = description;
        attributeName.color = Color.white;
        attributeValue.color = Color.white;
        background.color = Color.black;
        menu.SelectItem(parentTransform);
        StopAllCoroutines();
        StartCoroutine(ScaleAnimation(transform.localScale, initialScale * scaleMultiplier));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        square.color = Color.white;
        triangle.color= Color.black;
        attributeName.color = Color.black;
        attributeValue.color = Color.black;
        background.color = new Color(0, 0, 0, 0);
        StopAllCoroutines();
        transform.localPosition = centerPosition;
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

    public void LevelUp()
    {
        StopAllCoroutines();
        StartCoroutine(PressIn(transform.localScale));
    }

    IEnumerator PressIn(Vector3 currentScale)
    {
        float timer = pressInTime;
        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / pressInTime;
            transform.localScale = Vector3.Lerp(initialScale * pressInMinscale, currentScale, ratio);
            yield return new WaitForEndOfFrame();
        }

        timer = unPressTime;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / unPressTime;
            transform.localScale = Vector3.Lerp(initialScale * scaleMultiplier, initialScale * pressInMinscale, ratio);
            yield return new WaitForEndOfFrame();
        }
    }

    public void FailToLevelUp()
    {
        StartCoroutine(VibrateButton());
    }

    IEnumerator VibrateButton()
    {
        float timer = vibrateDuration;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float input = Mathf.Cos(timer * vibrateFreq) * 0.5f + 0.5f;
            transform.localPosition = Vector3.Lerp(leftPosition, rightPosition, input);
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = centerPosition;
    }
}
