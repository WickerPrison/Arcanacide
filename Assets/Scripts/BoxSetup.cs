using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteAlways]
public class BoxSetup : MonoBehaviour
{
    [SerializeField] bool useStackHeight;
    [SerializeField] int stackHeight;
    MeshRenderer mesh_renderer;
    MeshRenderer meshRenderer
    {
        get
        {
            if(mesh_renderer == null)
            {
                mesh_renderer = GetComponentInChildren<MeshRenderer>();
            }
            return mesh_renderer;
        }
    }



#if UNITY_EDITOR

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

    public void RandomRotateBox()
    {
        transform.localRotation = Quaternion.Euler(RandomAngle(), RandomAngle(), RandomAngle());
    }

    float RandomAngle()
    {
        int randInt = Random.Range(0, 4);
        return randInt * 90f;
    }

    public void ShowBox(bool show)
    {
        meshRenderer.enabled = show;
    }
#endif
}