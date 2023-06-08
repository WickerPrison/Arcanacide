using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class CubicleSpawner : MonoBehaviour
{
    [SerializeField] GameObject cubiclePrefab;
    [SerializeField] bool activate;
    [SerializeField] int cubicleNum;
    [SerializeField] bool doubleSided;
    Vector3 sideShift = new Vector3(-3.3f, 0, 0);
    Vector3 backShift = new Vector3(0, 0, 2.45f);

    private void OnDrawGizmosSelected()
    {
        if (activate)
        {
            activate = false;
            SpawnCubicles();
        }
    }

    void SpawnCubicles()
    {
        //for (int i = 0; i < cubicleNum; i++)
        //{
        //    GameObject cubicle = PrefabUtility.InstantiatePrefab(cubiclePrefab) as GameObject;
        //    cubicle.transform.parent = transform.parent;
        //    cubicle.transform.localPosition = transform.localPosition + sideShift * i;

        //    if (doubleSided)
        //    {
        //        GameObject backCubicle = PrefabUtility.InstantiatePrefab(cubiclePrefab) as GameObject;
        //        backCubicle.transform.parent = transform.parent;
        //        backCubicle.transform.localPosition = cubicle.transform.localPosition + backShift;
        //        backCubicle.transform.localScale = new Vector3(1, 1, -1);
        //    }
        //}
    }
}
