using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeamVfx : MonoBehaviour
{
    LineRenderer line;
    [SerializeField] LevelColor lazerColor;
    Color unchargedColor;
    [SerializeField] ColorData colorData;
    Gradient gradient;
    float alpha = 1;
    float aimValue;
    float maxAimValue = 3;
    float gradientOffset = .4f;
    Vector3 away = new Vector3(100, 100, 100);
    [SerializeField] LayerMask layerMask;

    private void Start()
    {
        unchargedColor = colorData.GetColor(lazerColor);
        line = GetComponent<LineRenderer>();
        HideBeam();

        gradient = new Gradient();
        GradientColorKey[] colorKey = { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(unchargedColor, 1.0f) };
        GradientAlphaKey[] alphaKey = { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) };
        gradient.SetKeys(colorKey, alphaKey);

        aimValue = maxAimValue;
    }

    public void SetMaxAimValue(float value)
    {
        maxAimValue = value;
        aimValue = maxAimValue;
    }

    public RaycastHit BeamHitscan(Vector3 origin, Vector3 target)
    {
        RaycastHit hit;
        Physics.Linecast(origin, target, out hit, layerMask, QueryTriggerInteraction.Ignore);
        return hit;
    }

    public void SetPositions(Vector3 origin, Vector3 endpoint)
    {
        line.SetPosition(0, origin);
        line.SetPosition(1, endpoint);
    }

    public void HideBeam()
    {
        line.SetPosition(0, away);
        line.SetPosition(1, away);
    }

    public void DecrementAimValue(Action chargedCallback)
    {
        aimValue -= Time.deltaTime;
        if(aimValue < -gradientOffset)
        {
            chargedCallback();
            aimValue = maxAimValue;
        }
        UpdateGradient();
    }

    public void ResetAimValue()
    {
        aimValue = maxAimValue;
        UpdateGradient();
    }

    void UpdateGradient()
    {
        float aimRatio = aimValue / maxAimValue;

        if (aimRatio < 1 - gradientOffset)
        {
            line.startColor = gradient.Evaluate(aimRatio + gradientOffset);
        }
        else
        {
            line.startColor = unchargedColor;
        }
        line.endColor = gradient.Evaluate(aimRatio);
    }
}
