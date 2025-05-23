using UnityEngine;
using UnityEditor;

public class LayoutSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Rotate Layout &L")]
    static void RotateLayout()
    {
        LayoutSetup layoutSetup = FindObjectOfType<LayoutSetup>();
        if (layoutSetup == null)
        {
            GameObject layoutObject = GameObject.Find("Layout");
            if(layoutObject != null)
            {
                layoutSetup = layoutObject.AddComponent<LayoutSetup>(); ;
            }
            else
            {
                Debug.LogWarning("No LayoutSetup Found");
                return;
            }
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
#endif
}
