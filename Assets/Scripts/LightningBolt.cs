using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    [SerializeField] int pointNum = 2;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    public LineRenderer lineRenderer;
    [SerializeField] float noiseAmp = 1;
    [SerializeField] List<LineRenderer> forks = new List<LineRenderer>();
    int forkPointNum = 5;
    int frameDivider = 3;
    public int frameCounter = 0;
    Vector3[] points;
    Vector3 direction;
    float length;

    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[pointNum];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(frameCounter < frameDivider)
        {
            frameCounter += 1;
            return;
        }

        direction = endPoint.position - startPoint.position;
        length = Vector3.Distance(startPoint.position, endPoint.position);

        frameCounter = 0;
        points[0] = startPoint.position;
        for(int i = 1; i < pointNum - 1; i++)
        {
            float distance = length * i / (pointNum - 1);
            Vector3 position = startPoint.position + direction.normalized * distance + Noise();
            points[i] = position;
        }
        points[pointNum - 1] = endPoint.position + Noise();
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        
        foreach(LineRenderer fork in forks)
        {
            PlaceFork(fork);
        }
    }

    Vector3 Noise()
    {
        Vector3 noiseVector;
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        noiseVector = new Vector3(x, y, z) * noiseAmp;
        return noiseVector;
    }

    void PlaceFork(LineRenderer fork)
    {
        int node = Random.Range(1, pointNum - 1);
        Vector3[] forkPoints = new Vector3[forkPointNum];
        Vector3 noiseVector = Noise() * 0.5f;
        forkPoints[0] = points[node];
        for(int i = 1; i < forkPointNum; i++)
        {
            forkPoints[i] = forkPoints[i - 1] + noiseVector + Noise() * 0.5f;
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
