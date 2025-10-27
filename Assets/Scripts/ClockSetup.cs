using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSetup : MonoBehaviour
{
    [SerializeField] Transform minuteHand;
    [SerializeField] Transform hourHand;
    WaitForSeconds minute = new WaitForSeconds(60);
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        int randSec = Random.Range(0, 60);
        StartCoroutine(TickTimer(new WaitForSeconds(randSec)));
    }

    void TickHand()
    {
        minuteHand.Rotate(new Vector3(0, 0, -6));
    }

    IEnumerator TickTimer(WaitForSeconds waitTime)
    {
        yield return waitTime;
        TickHand();
        StartCoroutine(TickTimer(minute));
    }

    public void RandomizeClock()
    {
        minuteHand.Rotate(new Vector3(0, 0, Random.Range(0, 365)));
        hourHand.Rotate(new Vector3(0, 0, Random.Range(0, 365)));
    }
}
