using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MinibossBladeAnimationEvents : MonoBehaviour
{
    [SerializeField] SortingGroup[] frontBlade;

    public void SetFrontBladeOrder(int order)
    {
        foreach(SortingGroup group in frontBlade)
        {
            group.sortingOrder = order;
        }
    }
}
