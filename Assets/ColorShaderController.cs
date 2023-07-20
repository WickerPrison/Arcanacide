using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShaderController : MonoBehaviour
{
    [SerializeField] Color spriteColor;
    [SerializeField] Material material;
    SpriteRenderer[] spriteRenderers;
    CameraFollow mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = material;
            renderer.material.SetColor("_OriginalColor", spriteColor);
            renderer.material.SetColor("_FloorColor", mainCamera.floorColor);
        }
    }
}
