using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BoxShelf : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float chanceOfEmpty;

#if UNITY_EDITOR
    public void RandomizeBoxes()
    {
        BoxSetup[] boxes = GetComponentsInChildren<BoxSetup>();
        foreach(BoxSetup box in boxes)
        {
            box.ShowBox(true);
            float randFloat = Random.Range(0f, 1f);
            if(randFloat < chanceOfEmpty)
            {
                box.ShowBox(false);
                continue;
            }
            box.RandomRotateBox();
            float zPos = Random.Range(0.35f, 0.55f);
            box.transform.localPosition = new Vector3(
                box.transform.localPosition.x,
                box.transform.localPosition.y,
                zPos * Mathf.Sign(box.transform.localPosition.z)
                );
        }
    }

#endif
}
