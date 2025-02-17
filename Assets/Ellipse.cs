using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellipse : MonoBehaviour
{
    [SerializeField] Transform a;
    [SerializeField] Transform b;
    [SerializeField] Transform point;
    Dictionary<Vector3, float> startingValueDict = new Dictionary<Vector3, float>();

    private void Start()
    {
        float percent = 2 * Mathf.PI / 8;
        for(int i = 0; i < 8; i++)
        {
            float t = percent * i;
            startingValueDict.Add(GetPosition(t), t);
        }
    }

    private void OnDrawGizmos()
    {
        int gizmoPoints = 10;
        float percent = 2 * Mathf.PI / gizmoPoints;
        for (int i = 0; i < gizmoPoints; i++)
        {
            float t = percent * i;
            Gizmos.DrawLine(GetPosition(t), GetPosition(t + percent));
        }
    }

    public Vector3 GetPosition(float t)
    {
        float x = a.transform.localPosition.x * Mathf.Cos(t);
        float y = b.transform.localPosition.z * Mathf.Sin(t);
        return Utils.RotateDirection(new Vector3(x, 0, y), -transform.eulerAngles.y) + transform.position;
    }

    public (Vector3, float) GetStartingPosition(Vector3 startPos)
    {
        Vector3 best = Vector3.positiveInfinity;
        float bestVal = Mathf.Infinity;
        foreach(Vector3 key in startingValueDict.Keys)
        {
            float thisDist = Vector3.Distance(startPos, key);
            if (thisDist < bestVal)
            {
                bestVal = thisDist;
                best = key;
            }
        }

        return (best, startingValueDict[best]);
    }
}
