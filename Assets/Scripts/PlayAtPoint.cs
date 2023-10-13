using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAtPoint : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip audioClip, float volume, Vector3 position)
    {
        transform.position = position;
        audioSource.PlayOneShot(audioClip, volume);
        StartCoroutine(DestroyAtEnd(audioClip.length));
    }

    IEnumerator DestroyAtEnd(float duration)
    {
        yield return new WaitForSeconds(duration + 0.1f);
        Destroy(gameObject);
    }
}
