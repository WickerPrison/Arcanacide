using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PosterType
{
    WORK_HARDER, BOOKS, WORDS_HURT, HANG_IN_THERE
}

public class PosterSetup : MonoBehaviour
{
    [SerializeField] PosterType posterOption;
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        SetSprite();
    }

    void SetSprite()
    {
        switch (posterOption)
        {
            case PosterType.WORK_HARDER:
                spriteRenderer.sprite = sprites[0];
                break;
            case PosterType.BOOKS:
                spriteRenderer.sprite = sprites[1];
                break;
            case PosterType.WORDS_HURT:
                spriteRenderer.sprite = sprites[2];
                break;
            case PosterType.HANG_IN_THERE:
                spriteRenderer.sprite = sprites[3];
                break;
        }
    }

    public void Randomize()
    {
        posterOption = (PosterType)Random.Range(0, System.Enum.GetValues(typeof(PosterType)).Length);
        SetSprite();
    }
}
