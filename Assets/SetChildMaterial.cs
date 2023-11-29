using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetChildMaterial : MonoBehaviour
{
    [SerializeField] bool on = false;
    [SerializeField] Material material;
    Image[] children; 

    private void OnDrawGizmosSelected()
    {
        if (!on) return;
        if(children == null)
        {
            children = GetComponentsInChildren<Image>();
        }

        foreach(Image child in children)
        {
            child.material = material;
        }
    }
}
