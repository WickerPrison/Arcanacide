using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[ExecuteAlways]
public class BoxSetup : MonoBehaviour
{
    [SerializeField] bool useStackHeight;
    [SerializeField] int stackHeight;

    private void OnValidate()
    {
        if (useStackHeight)
        {
            Undo.RecordObject(transform, "Stack Boxes");
            transform.localPosition = new Vector3(
                transform.localPosition.x, 
                transform.localScale.y * 0.5f + stackHeight * transform.localScale.y, 
                transform.localPosition.z);
            PrefabUtility.RecordPrefabInstancePropertyModifications(transform);
        }
    }
}
#endif