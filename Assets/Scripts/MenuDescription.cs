using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuDescription : MonoBehaviour
{
    [SerializeField] Transform description;
    float maxScale;
    [SerializeField] float speed = 25;

    private void Start()
    {
        maxScale = description.localScale.x;
        description.localScale = new Vector3(0, description.localScale.y, description.localScale.z);
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if(description.localScale.x < maxScale)
            {
                description.localScale = new Vector3(description.localScale.x + Time.deltaTime * speed, description.localScale.y, description.localScale.z);
                if(description.localScale.x > maxScale)
                {
                    description.localScale = new Vector3(maxScale, description.localScale.y, description.localScale.z);
                }
            }
        }
        else
        {
            if(description.localScale.x > 0)
            {
                description.localScale = new Vector3(description.localScale.x - Time.deltaTime * speed, description.localScale.y, description.localScale.z);
                if(description.localScale.x < 0)
                {
                    description.localScale = new Vector3(0, description.localScale.y, description.localScale.z);
                }
            }
        }
    }
}
