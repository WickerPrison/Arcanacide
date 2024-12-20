using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MapRoomSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] Vector2 size;
    float scaleFactor = 100;
    bool hasChanged = false;
    RectTransform square;
    RectTransform outline;

    private void OnEnable()
    {
        square = GetComponent<RectTransform>();
        outline = transform.Find("Outline").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (hasChanged)
        {
            square.sizeDelta = size * scaleFactor;
            outline.sizeDelta = size * scaleFactor;
            hasChanged = false;
        }
    }

    private void OnValidate()
    {
        hasChanged = true;
    }
#endif
}
