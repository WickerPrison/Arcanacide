using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMachine : MonoBehaviour
{
    [SerializeField] GameObject fireWavePrefab;
    [SerializeField] Transform target;
    [SerializeField] float initialDelay;
    [SerializeField] float interWaveDelay = 1f;
    [SerializeField] float waveDelay = 5f;
    [SerializeField] bool silentWaves;
    [SerializeField] SpriteRenderer grate;
    [SerializeField] Color redColor;
    [SerializeField] float fireWaveSpeed;
    [SerializeField] float spawnOffset;
    float waveTime;
    int waveNum = 2;

    // Start is called before the first frame update
    void Start()
    {
        waveTime = initialDelay;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(waveTime > 0)
        {
            waveTime -= Time.deltaTime;
            if (waveTime < 0.5f)
            {
                grate.color = redColor;
            }
        }
        else
        {
            FireWave();
            if(waveNum > 0)
            {
                waveTime = interWaveDelay;
                waveNum -= 1;
            }
            else
            {
                waveTime = waveDelay;
                waveNum = 2;
            }
        }
    }

    public void FireWave()
    {
        GameObject fireWave;
        fireWave = Instantiate(fireWavePrefab);
        Vector3 direction = Vector3.Normalize(target.position - transform.position);
        fireWave.transform.position = transform.position + direction * spawnOffset;
        FireWave fireWaveScript;
        fireWaveScript = fireWave.GetComponent<FireWave>();
        fireWaveScript.target = target.position;
        fireWaveScript.moveSpeed = 0;
        StartCoroutine(DelayedFireWave(fireWaveScript));
        if (silentWaves && waveNum != 2)
        {
            AudioSource audio = fireWave.GetComponent<AudioSource>();
            audio.Stop();
        }
    }


    IEnumerator DelayedFireWave(FireWave fireWaveScript)
    {
        yield return new WaitForSeconds(0.5f);
        fireWaveScript.moveSpeed = fireWaveSpeed;
        if(waveNum == 2)
        grate.color = Color.white;
    }
}
