using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodstain : MonoBehaviour
{
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int randInt = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randInt];
        float randAngle = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, randAngle));
        float randSize = Random.Range(0.8f, 1.2f);
        transform.localScale = new Vector3(randSize, randSize, randSize);
        StartCoroutine(Fadeout());
    }

    IEnumerator Fadeout()
    {
        yield return new WaitForSeconds(1);

        float fadeTime = 1f;
        float timer = fadeTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.color = Color.Lerp(endColor, startColor, timer);
            yield return endOfFrame;
        }
        Destroy(gameObject);
    }
}
