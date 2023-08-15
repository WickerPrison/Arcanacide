using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVibrate : MonoBehaviour
{
    Vector3 centerPosition;
    Vector3 leftPosition;
    Vector3 rightPosition;
    float vibrateAmp = 5;
    float vibrateDuration = 0.3f;
    float vibrateFreq = 40;

    private void Start()
    {
        centerPosition = transform.localPosition;
        leftPosition = centerPosition - Vector3.right * vibrateAmp;
        rightPosition = centerPosition + Vector3.right * vibrateAmp;
    }

    public void StartVibrate()
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
