using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuAnimations : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] ParticleSystem particles;
    float closedPos = -2.95f;
    float openPos = 8.76f;
    bool doorClosed = false;
    float doorTime = 3f;

    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    private void Start()
    {
        door.localPosition = new Vector3(openPos, 0, 0);
        StartCoroutine(DoorWait(3));
    }

    IEnumerator DoorWait(float wait)
    {
        ParticlesOnOff();
        yield return new WaitForSeconds(wait);
        if(doorClosed)
        {
            StartCoroutine(OpenCloseDoor(openPos));
        }
        else
        {
            StartCoroutine(OpenCloseDoor(closedPos));
        }
        doorClosed = !doorClosed;
        ParticlesOnOff(); 
    }

    IEnumerator OpenCloseDoor(float finalPositionX)
    {
        Vector3 finalPosition = new Vector3(finalPositionX, 0, 0);
        Vector3 startPosition = door.localPosition;
        float timer = doorTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            door.localPosition = Vector3.Lerp(finalPosition, startPosition, timer / doorTime);
            yield return endOfFrame;
        }
        float randTime = Random.Range(4, 30);
        StartCoroutine(DoorWait(randTime));
    }

    void ParticlesOnOff()
    {
        if (doorClosed)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }
}
