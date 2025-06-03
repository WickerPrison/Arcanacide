using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteAttack : MonoBehaviour
{
    [SerializeField] SpriteRenderer tallIcicle;
    [SerializeField] Vector3 tallIcicleStart;
    [SerializeField] Vector3 tallIcicleFinal;
    [SerializeField] SpriteRenderer bigIcicle;
    [SerializeField] Vector3 bigIcicleStart;
    [SerializeField] Vector3 bigIcicleFinal;
    [SerializeField] float emergeTime;

    // Start is called before the first frame update
    void Start()
    {
        int randInt = Random.Range(0, 2);
        tallIcicle.transform.localPosition = tallIcicleStart;
        bigIcicle.transform.localPosition = bigIcicleStart;
        if(randInt == 0)
        {
            tallIcicle.enabled = true;
            bigIcicle.enabled = false;
            StartCoroutine(Emerge(tallIcicle, tallIcicleStart, tallIcicleFinal));
        }
        else
        {
            tallIcicle.enabled = false;
            bigIcicle.enabled = true;
            StartCoroutine(Emerge(bigIcicle, bigIcicleStart, bigIcicleFinal));
        }
    }

    IEnumerator Emerge(SpriteRenderer icicle, Vector3 start, Vector3 end)
    {
        float emergeTimer = 0;
        while(emergeTimer < emergeTime)
        {
            icicle.transform.localPosition = Vector3.Lerp(start, end, emergeTimer / emergeTime);
            emergeTimer += Time.deltaTime;
            yield return null;
        }
    }
}
