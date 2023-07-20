using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorChange : MonoBehaviour
{
    [SerializeField] Color spriteColor;
    Image image;
    CameraFollow mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.material.SetColor("_OldColor", spriteColor);
        image.material.SetColor("_NewColor", mainCamera.floorColor);        
    }
}
