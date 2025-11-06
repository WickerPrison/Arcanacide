using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float GetAngle(Vector3 vectorA, Vector3 vectorB)
    {
        float angle = Mathf.Acos(Vector3.Dot(vectorA, vectorB) / (vectorA.magnitude * vectorB.magnitude));
        return angle * Mathf.Rad2Deg;
    }

    public static float GetAngle(Vector3 vector)
    {
        return GetAngle(vector, Vector3.up);
    }

    public static float GetAngle(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Mathf.Acos(Vector3.Dot(vectorA, vectorB) / (vectorA.magnitude * vectorB.magnitude));
        return angle * Mathf.Rad2Deg;
    }

    public static float GetAngle(Vector2 vector)
    {
        return GetAngle(vector, Vector2.up);
    }

    public static Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
    }

    public static void DrawDebugCircle(int pointNum, float radius, Vector3 center, float duration = 5)
    {
        float percent = 360 / pointNum;
        for (int i = 0; i < pointNum; i++)
        {
            float t = percent * i;
            Vector3 pos1 = RotateDirection(Vector3.right, t).normalized * radius + center;
            Vector3 pos2 = RotateDirection(Vector3.right, t + percent).normalized * radius + center;
            Debug.DrawLine(pos1, pos2, Color.red, duration);
        }
    }
}