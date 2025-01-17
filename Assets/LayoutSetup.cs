using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LayoutSetup : MonoBehaviour
{
    [MenuItem("Tools/Rotate Layout &L")]
    static void RotateLayout()
    {
        LayoutSetup layoutSetup = FindObjectOfType<LayoutSetup>();
        if (layoutSetup == null)
        {
            Debug.LogWarning("No LayoutSetup Found");
            return;
        }
        Transform transform = layoutSetup.transform;
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
