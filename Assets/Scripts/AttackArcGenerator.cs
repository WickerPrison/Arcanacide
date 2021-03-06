using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArcGenerator : MonoBehaviour
{
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material colorMaterial;
    [SerializeField] GameObject viewConeObject;
    [SerializeField] MeshCollider colliderMesh;
    [SerializeField] int halfConeAngle;
    [SerializeField] float radius;
    [SerializeField] float yOffset;
    EnemyController enemyController;
    Material viewConeMaterial;
    Renderer coneRenderer;

    public MeshFilter meshFilter;
    Mesh viewMesh;

    int arcPoints;
    Vector3 centerPoint;   

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        centerPoint = new Vector3(0, yOffset, -1);
        viewConeMaterial = new Material(whiteMaterial);
        coneRenderer = viewConeObject.GetComponent<Renderer>();
        coneRenderer.material = viewConeMaterial;
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        meshFilter.mesh = viewMesh;
        colliderMesh.sharedMesh = viewMesh;
        arcPoints = halfConeAngle * 2;
        CalculateAttackArc();
        coneRenderer.enabled = false;
    }

    public void CalculateAttackArc()
    {
        Vector3[] vertices = new Vector3[arcPoints + 3];
        int[] triangles = new int[(arcPoints + 3) * 3];
        vertices[0] = new Vector3(-0.3f,yOffset,-0.5f);
        for(int i = 0; i <= arcPoints - 1; i++)
        {
            Vector3 nextPosition;
            nextPosition = FindNextPosition(i - halfConeAngle);
            vertices[i + 1] = nextPosition;
            if(i < arcPoints - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        vertices[arcPoints] = new Vector3(0.3f, yOffset, -0.5f);
        triangles[arcPoints * 3] = 0;
        triangles[arcPoints * 3 + 1] = arcPoints + 1;
        triangles[arcPoints * 3 + 2] = arcPoints + 2;
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
        coneRenderer.material = whiteMaterial;
    }

    public void ShowAttackArc()
    {
        coneRenderer.enabled = true;
    }

    public void HideAttackArc()
    {
        coneRenderer.material = whiteMaterial;
        coneRenderer.enabled = false;
    }

    public void ColorArc()
    {
        coneRenderer.material = colorMaterial;
    }

    Vector3 FindNextPosition(float angle)
    {
        float angleToZero = Mathf.Acos(Vector3.Dot(Vector3.forward, transform.forward) / (Vector3.forward.magnitude * transform.forward.magnitude));
        if (transform.forward.x >= 0)
        {
            angle += angleToZero * Mathf.Rad2Deg;
        }
        else
        {
            angle -= angleToZero * Mathf.Rad2Deg;
        }
        Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
        direction = direction.normalized * radius;
        Vector3 nextPosition = centerPoint + direction;
        return nextPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyController.canHitPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyController.canHitPlayer = false;
        }
    }
}
