using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryAudioSourceEvents : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void PlaySecondaryAudio()
    {
        source.Play();
    }

    public void PauseSecondaryAudio()
    {
        source.Pause();
    }
}
