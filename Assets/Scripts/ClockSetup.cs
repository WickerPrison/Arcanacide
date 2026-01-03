using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSetup : MonoBehaviour
{
    [SerializeField] Transform minuteHand;
    [SerializeField] Transform hourHand;
    [SerializeField] bool chaosClock = false;
    WaitForSeconds minute = new WaitForSeconds(60);
    float angle;
    float hourSpeed;
    float minuteSpeed;

    // Start is called before the first frame update
    void Start()
    {
        int randSec = Random.Range(0, 60);
        if (!chaosClock)
        {
            StartCoroutine(TickTimer(new WaitForSeconds(randSec)));
        }
        else
        {
            hourSpeed = Random.Range(-5, 5);
            minuteSpeed = Random.Range(-20, 20);
        }
    }

    private void Update()
    {
        if (!chaosClock) return;
        minuteHand.Rotate(new Vector3(0, 0, minuteSpeed * Time.deltaTime));
        hourHand.Rotate(new Vector3(0, 0, hourSpeed * Time.deltaTime));
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
