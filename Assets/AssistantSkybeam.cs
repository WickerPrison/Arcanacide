using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantSkybeam : MonoBehaviour
{
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] Transform skybeam;
    [SerializeField] float skybeamTime;
    [SerializeField] float initialDelay;
    [SerializeField] int rippleNum;
    [SerializeField] float rippleDelayTime;
    [SerializeField] Color rippleColor;
    WaitForSeconds rippleDelay;
    WaitForEndOfFrame endOfFrame;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Skybeam());
    }

    IEnumerator Skybeam()
    {
        endOfFrame = new WaitForEndOfFrame();
        yield return new WaitForSeconds(initialDelay);
        float timer = skybeamTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;

            skybeam.localScale = Vector3.Lerp(Vector3.one, new Vector3(1, 0, 1), timer / skybeamTime);

            yield return endOfFrame;
        }
        StartCoroutine(Ripples());
    }

    IEnumerator Ripples()
    {
        rippleDelay = new WaitForSeconds(rippleDelayTime);
        for (int i = 0; i < rippleNum; i++)
        {
            IceRipple ripple = Instantiate(ripplePrefab).GetComponent<IceRipple>();
            ripple.transform.position = transform.position + Vector3.up;
            ripple.transform.localScale = Vector3.one * 0.5f;
            ripple.startRadius = 0.5f;
            ripple.numberOfBoxes = 30;
            ripple.rippleSpeed = 5;
            ripple.lifeTime = 3;
            ripple.boxColor = rippleColor;
            yield return rippleDelay;
        }
        yield return rippleDelay;
        Destroy(gameObject);
    }
}
