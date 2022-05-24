using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField] Texture2D characterTexture2D;
    [SerializeField] Color oldColor;
    [SerializeField] Color newColor;

    private void Start()
    {
        UpdateCharacterTexture();
    }

    //CopiedTexture is the original Texture  which you want to copy.
    public Texture2D CopyTexture2D(Texture2D copiedTexture)
    {
        //Create a new Texture2D, which will be the copy.
        Texture2D texture = new Texture2D(copiedTexture.width, copiedTexture.height);
        //Choose your filtermode and wrapmode here.
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        int y = 0;
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {
                //INSERT YOUR LOGIC HERE
                if (copiedTexture.GetPixel(x, y) == oldColor)
                {
                    //This line of code and if statement, turn Green pixels into Red pixels.
                    texture.SetPixel(x, y, newColor);
                }
                else
                {
                    //This line of code is REQUIRED. Do NOT delete it. This is what copies the image as it was, without any change.
                    texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
                }
                ++x;
            }
            ++y;
        }
        //Name the texture, if you want.
        //texture.name = (Species + Gender + "_SpriteSheet");

        //This finalizes it. If you want to edit it still, do it before you finish with .Apply(). Do NOT expect to edit the image after you have applied. It did NOT work for me to edit it after this function.
        texture.Apply();

        //Return the variable, so you have it to assign to a permanent variable and so you can use it.
        return texture;
    }

    public void UpdateCharacterTexture()
    {
        //This calls the copy texture function, and copies it. The variable characterTextures2D is a Texture2D which is now the returned newly copied Texture2D.
        characterTexture2D = CopyTexture2D(gameObject.GetComponent<SpriteRenderer>().sprite.texture);

        //Get your SpriteRenderer, get the name of the old sprite,  create a new sprite, name the sprite the old name, and then update the material. If you have multiple sprites, you will want to do this in a loop- which I will post later in another post.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string tempName = spriteRenderer.sprite.name;
        spriteRenderer.sprite = Sprite.Create(characterTexture2D, spriteRenderer.sprite.rect, new Vector2(0, 1));
        spriteRenderer.sprite.name = tempName;

        spriteRenderer.material.mainTexture = characterTexture2D;
        spriteRenderer.material.shader = Shader.Find("Sprites/Transparent Unlit");

    }
}
