using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArcUtils
{
    public struct ArcData
    {
        public Vector3 startPoint;
        public Vector3 endPoint;
        public Vector3 direction;
        public float speed;
        public float a;
        public float b;
        public float c;
    }

    public static ArcData CreateArcData(Vector3 startPoint, Vector3 endPoint, float timeToHit, float arcHeight, float thirdPointX = 0.3f)
    {
        ArcData arcData = new ArcData();
        arcData.startPoint = startPoint;
        arcData.endPoint = endPoint;

        arcData.direction = new Vector3(endPoint.x, 0, endPoint.z) - new Vector3(startPoint.x, 0, startPoint.z);

        float totDist = Vector2.Distance(new Vector2(startPoint.x, startPoint.z), new Vector2(endPoint.x, endPoint.z));

        arcData.speed = totDist / timeToHit;

        float w2 = Mathf.Pow(thirdPointX, 2);
        arcData.a = -((arcHeight + startPoint.y * w2) / (thirdPointX - w2));
        arcData.b = (arcHeight + startPoint.y * w2) / (thirdPointX - w2) - startPoint.y;
        arcData.c = startPoint.y;
        return arcData;
    }

    public static Vector3 GetNextArcPosition(Vector3 currentPosition, ArcData arcData)
    {
        Vector3 nextPosition = currentPosition + arcData.direction.normalized * Time.fixedDeltaTime * arcData.speed;
        float xVal = InverseLerpSetY0(arcData.startPoint, arcData.endPoint, nextPosition);
        float nextHeight = arcData.a * Mathf.Pow(xVal, 2) + arcData.b * xVal + arcData.c;
        return new Vector3(nextPosition.x, nextHeight, nextPosition.z);
    }

    public static float InverseLerpSetY0(Vector3 startPos, Vector3 endPos, Vector3 currentPos)
    {
        endPos = new Vector3(endPos.x, 0, endPos.z);
        startPos = new Vector3(startPos.x, 0, startPos.z);
        currentPos = new Vector3(currentPos.x, 0, currentPos.z);
        Vector3 diff = endPos - startPos;
        Vector3 currentDiff = currentPos - startPos;
        return Vector3.Dot(currentDiff, diff) / Vector3.Dot(diff, diff);
    }
}
