using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordGlow : MonoBehaviour
{
    PlayerEvents playerEvents;
    Vector3 startScale = new Vector3(1.2f, 1.2f, 1f);
    Vector3 maxScale = new Vector3(2f, 1.25f, 1f);
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    PlayerScript playerScript;

    private void Awake()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
        playerScript = GetComponentInParent<PlayerScript>();
    }

    private void OnEnable()
    {
        playerEvents.onSwordHeavyFullCharge += onSwordHeavyFullCharge;
    }

    private void OnDisable()
    {
        playerEvents.onSwordHeavyFullCharge -= onSwordHeavyFullCharge;
    }

    private void onSwordHeavyFullCharge(object sender, System.EventArgs e)
    {
        StartCoroutine(SwordPulse());
    }

    IEnumerator SwordPulse()
    {
        transform.localScale = startScale;
        float pulseUpTime = 0.1f;
        float timer = pulseUpTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(maxScale, startScale, timer / pulseUpTime);
            yield return endOfFrame;
        }

        float pulseDownTime = 0.1f;
        timer = pulseDownTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, maxScale, timer / pulseDownTime);
            yield return endOfFrame;
        }
        if (playerScript.testingEvents != null) playerScript.testingEvents.FullyCharged();
    }
}
