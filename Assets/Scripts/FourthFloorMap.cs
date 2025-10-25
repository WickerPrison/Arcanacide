using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FourthFloorMap : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] GameObject[] maps;
    [SerializeField] MapData mapData;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    int recent;

    // Start is called before the first frame update
    void Start()
    {
        material.SetColor("_NewColor", mapData.floorColor);

        SelectMap();

        StartCoroutine(GlitchEffect());
    }

    IEnumerator GlitchEffect()
    {
        float waitTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(waitTime);

        float rampUp = 0.4f;
        float timer = rampUp;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float amount = 1 - timer / rampUp;
            material.SetFloat("_Amount", amount);
            yield return endOfFrame;
        }

        float duration = Random.Range(0.05f, 0.2f);
        yield return new WaitForSeconds(duration);
        SelectMap();

        float rampDown = 0.2f;
        timer = rampDown;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float amount = timer / rampDown;
            if (amount < 0) amount = 0;
            material.SetFloat("_Amount", amount);
            yield return endOfFrame;
        }

        StartCoroutine(GlitchEffect());
    }

    void SelectMap()
    {
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }

        int randMap = Random.Range(0, maps.Length);
        while(randMap == recent)
        {
            randMap = Random.Range(0, maps.Length);
        }
        recent = randMap;
        maps[randMap].SetActive(true);
    }
}
