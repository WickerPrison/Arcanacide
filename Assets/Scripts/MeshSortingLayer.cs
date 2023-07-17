using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSortingLayer : MonoBehaviour
{
    [SerializeField] string sortingLayerName;
    [SerializeField] int sortingOrder;
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;
    }
}
