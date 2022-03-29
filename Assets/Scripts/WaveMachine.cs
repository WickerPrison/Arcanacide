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
    float waveTime;
    int waveNum = 2;

    // Start is called before the first frame update
    void Start()
    {
        waveTime = initialDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(waveTime > 0)
        {
            waveTime -= Time.deltaTime;
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

    void FireWave()
    {
        GameObject fireWave;
        fireWave = Instantiate(fireWavePrefab);
        fireWave.transform.position = transform.position;
        FireWave fireWaveScript;
        fireWaveScript = fireWave.GetComponent<FireWave>();
        fireWaveScript.target = target.position;
        if (silentWaves && waveNum != 2)
        {
            AudioSource audio = fireWave.GetComponent<AudioSource>();
            audio.Stop();
        }
    }
}
