using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorChange : MonoBehaviour
{
    public bool colorChange = false;
    [SerializeField] ColorData colorData;
    [SerializeField] Color oldColor;
    [SerializeField] LevelColor newLevelColor;
    Color newColor;
    [SerializeField] List<SpriteRenderer> renderers;

    // Start is called before the first frame update
    void Start()
    {
        if (!colorChange) return;

        newColor = colorData.GetColor(newLevelColor);
        foreach (SpriteRenderer sprite in renderers)
        {
            sprite.material.SetFloat("_ColorChange", 1);
            sprite.material.SetColor("_OriginalColor", oldColor);
            sprite.material.SetColor("_NewColor", newColor);
        }
    }
}
