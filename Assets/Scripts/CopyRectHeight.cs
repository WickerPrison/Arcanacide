using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRectHeight : MonoBehaviour
{
    [SerializeField] RectTransform objectToCopy;
    RectTransform myRectTransform;

    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        myRectTransform.sizeDelta = new Vector2(myRectTransform.sizeDelta.x, objectToCopy.sizeDelta.y * objectToCopy.localScale.y);
    }
}
