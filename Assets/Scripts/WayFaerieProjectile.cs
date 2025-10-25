using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayFaerieProjectile : MonoBehaviour
{
    [System.NonSerialized] public WayFairie destination;
    [System.NonSerialized] public Vector3 start;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float speed;
    [SerializeField] ParticleSystem particles;
    float maxTime;
    float time;

    private void Start()
    {
        transform.position = start;
        particles.Play();
        float dist = Vector3.Distance(start, destination.transform.position);
        maxTime = dist / speed;
        time = maxTime;
    }

    private void Update()
    {
        if (time <= 0) return;
        time -= Time.deltaTime;
        float progress = curve.Evaluate(time / maxTime);
        transform.position = Vector3.Lerp(destination.transform.position, start, progress);
        if(time <= 0)
        {
            StartCoroutine(DestroyWayFaerieProjectile());
        }
    }

    IEnumerator DestroyWayFaerieProjectile()
    {
        destination.ShowWayfinder();
        particles.Stop();
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
