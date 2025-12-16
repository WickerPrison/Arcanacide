using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossBlade : MonoBehaviour
{
    [SerializeField] int bladeId;
    [SerializeField] Vector3 outPos;
    [SerializeField] Vector3 outRot;
    [SerializeField] Vector3 outPos2;
    [SerializeField] Vector3 outRot2;
    [SerializeField] Vector3 inPos;
    [SerializeField] Vector3 inRot;
    [SerializeField] Vector3 inPos2;
    [SerializeField] Vector3 inRot2;
    [Range(0f, 1f)]
    [SerializeField] float retraction;
    [SerializeField] Transform blade1;
    [SerializeField] Transform blade2;
    [SerializeField] bool setBladePositions;
    MinibossEvents minibossEvents;
    float bladeTime = 0.2f;

    private void Awake()
    {
        minibossEvents = GetComponentInParent<MinibossEvents>();
    }

    private void OnValidate()
    {
        if (!setBladePositions) return;
        blade1.localPosition = Vector3.Lerp(outPos, inPos, retraction);
        blade1.localRotation = Quaternion.Euler(Vector3.Lerp(outRot, inRot, retraction));

        blade2.localPosition = Vector3.Lerp(outPos2, inPos2, retraction);
        blade2.localRotation = Quaternion.Euler(Vector3.Lerp(outRot2, inRot2, retraction));
    }

    private void MinibossEvents_onExtendBlades(object sender, int id)
    {
        if (bladeId != id) return;
        StopAllCoroutines();
        StartCoroutine(ExtendRetractBlades(outPos, outPos2, outRot, outRot2));
    }

    private void MinibossEvents_onRetractBlades(object sender, int id)
    {
        if (bladeId != id) return;
        StopAllCoroutines();
        StartCoroutine(ExtendRetractBlades(inPos, inPos2, inRot, inRot2));
    }

    IEnumerator ExtendRetractBlades(Vector3 endPos1, Vector3 endPos2, Vector3 endRot1, Vector3 endRot2)
    {
        Vector3 startPos1 = blade1.localPosition;
        Vector3 startPos2 = blade2.localPosition;
        Vector3 startRot1 = blade1.localEulerAngles;
        Vector3 startRot2 = blade2.localEulerAngles;

        Debug.Log($"Start Rot1: {startRot1}, In Rot1: {inRot}");
        float timer = bladeTime;
        while(timer > 0)
        {
            float rat = 1 - timer / bladeTime;
            blade1.localPosition = Vector3.Lerp(startPos1, endPos1, rat);
            blade1.localRotation = Quaternion.Euler(Vector3.Lerp(startRot1, endRot1, rat));

            blade2.localPosition = Vector3.Lerp(startPos2, endPos2, rat);
            blade2.localRotation = Quaternion.Euler(Vector3.Lerp(startRot2, endRot2, rat));
            Debug.Log(rat);
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnEnable()
    {
        minibossEvents.onRetractBlades += MinibossEvents_onRetractBlades;
        minibossEvents.onExtendBlades += MinibossEvents_onExtendBlades;
    }

    private void OnDisable()
    {
        minibossEvents.onRetractBlades -= MinibossEvents_onRetractBlades;
        minibossEvents.onExtendBlades -= MinibossEvents_onExtendBlades;
    }
}
