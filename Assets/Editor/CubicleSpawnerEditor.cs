using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CubicleSpawnerEditor : EditorWindow
{
    [MenuItem("Tools/Cubicle Spawner")]
    public static void OpenWindow() => GetWindow<CubicleSpawnerEditor>("Cubicle Spawner");
    int cubicleNum = 0;
    bool doubleSided = false;
    Vector3 sideShift = new Vector3(-3.3f, 0, 0);
    Vector3 backShift = new Vector3(0, 0, 2.45f);

    GameObject CubiclePrefab;
    GameObject cubiclePrefab
    {
        get
        {
            if(CubiclePrefab == null)
            {
                CubiclePrefab = (GameObject)AssetDatabase
                    .LoadAssetAtPath("Assets/Resources/Prefabs/Layout/Cubicle.prefab", typeof(GameObject));
            }
            return CubiclePrefab;
        }
    }

    private void OnGUI()
    {
        cubicleNum = EditorGUILayout.IntField("Cubicle Num", cubicleNum);
        doubleSided = EditorGUILayout.Toggle("Double Sided", doubleSided);
        if(GUILayout.Button("Spawn Cubicles"))
        {
            Transform cubicleHolder = new GameObject().transform;
            cubicleHolder.gameObject.name = "Cubicle Holder";
            cubicleHolder.parent = Selection.activeTransform;
            float centerShiftX = sideShift.x * ((float)cubicleNum / 2 - 0.5f);
            float centerShiftZ = 0;
            if (doubleSided) centerShiftZ = backShift.z / 2;
            Vector3 centerShift = new Vector3(centerShiftX, 0, centerShiftZ);
            for (int i = 0; i < cubicleNum; i++)
            {
                GameObject cubicle = PrefabUtility.InstantiatePrefab(cubiclePrefab) as GameObject;
                cubicle.transform.parent = cubicleHolder;
                cubicle.transform.localPosition = cubicleHolder.localPosition + sideShift * i - centerShift;

                if (doubleSided)
                {
                    GameObject backCubicle = PrefabUtility.InstantiatePrefab(cubiclePrefab) as GameObject;
                    backCubicle.transform.parent = cubicleHolder;
                    backCubicle.transform.localPosition = cubicle.transform.localPosition + backShift;
                    backCubicle.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }
            Selection.activeTransform = cubicleHolder;
        }
    }
}
