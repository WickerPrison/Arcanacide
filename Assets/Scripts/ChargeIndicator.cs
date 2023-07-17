using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeIndicator : MonoBehaviour
{
    public Vector3 finalPosition;
    public Vector3 initialPosition;
    public Vector3 initialNormal;
    public Vector3 finalNormal;
    public float indicatorWidth;
    [SerializeField] Material whiteMaterial;
    Material viewConeMaterial;
    Renderer coneRenderer;

    public MeshFilter meshFilter;
    [System.NonSerialized] public Mesh viewMesh;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        ReStart();
    }

    public void ReStart()
    {
        viewConeMaterial = new Material(whiteMaterial);
        coneRenderer = gameObject.GetComponent<Renderer>();
        coneRenderer.material = viewConeMaterial;
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        meshFilter.mesh = viewMesh;
        direction = initialPosition - finalPosition;
        CalculateAttackArc();
    }

    public void CalculateAttackArc()
    {

        Vector3 right = Quaternion.AngleAxis(-90, Vector3.up) * direction;
        float initialTheta = Vector3.SignedAngle(direction, initialNormal, Vector3.up) * Mathf.Deg2Rad;
        float initialOffset = indicatorWidth / 2 * Mathf.Tan(initialTheta);
        float finalTheta = Vector3.SignedAngle(direction, finalNormal, Vector3.up) * Mathf.Deg2Rad;
        float finalOffset = indicatorWidth / 2 * Mathf.Tan(finalTheta);

        Vector3[] vertices = new Vector3[]
        {
            initialPosition - right.normalized * indicatorWidth / 2 - direction.normalized * initialOffset,
            finalPosition - right.normalized * indicatorWidth / 2 - direction.normalized * finalOffset,
            finalPosition + right.normalized * indicatorWidth / 2 + direction.normalized * finalOffset,
            initialPosition + right.normalized * indicatorWidth / 2 + direction.normalized * initialOffset
        };


        int[] triangles = new int[] { 0, 1, 3, 3, 1, 2};

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
        coneRenderer.material = whiteMaterial;
    }
}
