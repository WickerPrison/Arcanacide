using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LayoutSetup : MonoBehaviour
{
    [MenuItem("Tools/Rotate Layout &L")]
    static void RotateLayout()
    {
        Transform transform = FindObjectOfType<LayoutSetup>().transform;
        Vector3 newEulerAngles = transform.eulerAngles;
        if(newEulerAngles.y == 0)
        {
            newEulerAngles.y = -45;
        }
        else
        {
            newEulerAngles.y = 0;
        }
        Undo.RecordObject(transform, "Rotate Layout");
        transform.eulerAngles = newEulerAngles;
        PrefabUtility.RecordPrefabInstancePropertyModifications(transform);
    }
}
