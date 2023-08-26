using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image bar;
    [System.NonSerialized] public bool on;
    [SerializeField] Vector2 onPos;
    [SerializeField] Vector2 offPos;
    float switchTime = 0.1f;
    [SerializeField] Image inner;
    [SerializeField] Image outer;

    public void ToggleSwitch(bool turnOn)
    {

        StartCoroutine(SwitchAnimation(turnOn));
    }

    IEnumerator SwitchAnimation(bool turnOn)
    {
        Vector2 targetPos;
        float barTarget;
        if (turnOn)
        {
            targetPos = onPos;
            barTarget = 1;
        }
        else
        {
            targetPos = offPos;
            barTarget = 0;
        }

        Vector2 pos = transform.localPosition;
        float timer = switchTime;
        float ratio;
        float currentScale = bar.transform.localScale.x;
        float barScale;

        while(switchTime > 0)
        {
            timer -= Time.unscaledDeltaTime;
            ratio = timer / switchTime;

            transform.localPosition = Vector2.Lerp(targetPos, pos, ratio);
            
            barScale = Mathf.Lerp(barTarget, currentScale, ratio);
            bar.transform.localScale = new Vector3(barScale, bar.transform.localScale.y, bar.transform.localScale.z);

            yield return new WaitForEndOfFrame();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        inner.color = Color.black;
        outer.color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        inner.color = Color.white;
        outer.color = Color.black;
    }
}
