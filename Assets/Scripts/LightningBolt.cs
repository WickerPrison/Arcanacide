using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public int pointNum = 2;
    public Transform startPoint;
    public Transform endPoint;
    public LineRenderer lineRenderer;
    public float noiseAmp = 1;
    public float endNoiseAmp = 1;
    public List<LineRenderer> forks = new List<LineRenderer>();
    int forkPointNum = 5;
    [System.NonSerialized] public int frameDivider = 3;
    public int frameCounter = 0;
    [System.NonSerialized] public Vector3[] points;
    Vector3 direction;
    float length;

    // Start is called before the first frame update
    public virtual void Start()
    {
        points = new Vector3[pointNum];
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(frameCounter < frameDivider)
        {
            frameCounter += 1;
            if(endPoint != null)
            {
                points[pointNum - 1] = endPoint.position + Noise(endNoiseAmp);
            }
            lineRenderer.SetPositions(points);
            return;
        }

        frameCounter = 0;
        PlacePoints();
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        
        foreach(LineRenderer fork in forks)
        {
            PlaceFork(fork);
        }
    }

    public virtual void PlacePoints()
    {
        direction = endPoint.position - startPoint.position;
        length = Vector3.Distance(startPoint.position, endPoint.position);
        points[0] = startPoint.position;
        for (int i = 1; i < pointNum - 1; i++)
        {
            float distance = length * i / (pointNum - 1);
            Vector3 position = startPoint.position + direction.normalized * distance + Noise(noiseAmp);
            points[i] = position;
        }
        points[pointNum - 1] = endPoint.position + Noise(endNoiseAmp);
    }

    public Vector3 Noise(float noise)
    {
        Vector3 noiseVector;
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        noiseVector = new Vector3(x, y, z) * noise;
        return noiseVector;
    }

    void PlaceFork(LineRenderer fork)
    {
        int node = Random.Range(1, pointNum - 1);
        Vector3[] forkPoints = new Vector3[forkPointNum];
        Vector3 noiseVector = Noise(noiseAmp) * 0.5f;
        forkPoints[0] = points[node];
        for(int i = 1; i < forkPointNum; i++)
        {
            forkPoints[i] = forkPoints[i - 1] + noiseVector + Noise(noiseAmp) * 0.5f;
        }

        fork.positionCount = forkPoints.Length;
        fork.SetPositions(forkPoints);
    }

    public void SetPositions(Vector3 startPosition, Vector3 endPosition)
    {
        startPoint.position = startPosition;
        endPoint.position = endPosition;
    }
}
