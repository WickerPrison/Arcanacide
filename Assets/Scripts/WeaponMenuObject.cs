using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class WeaponMenuObject : MonoBehaviour
{
    [SerializeField] MenuWeaponSelected weapon;
    [SerializeField] Vector3 awayPosition;
    [SerializeField] Vector3 selectedPosition;
    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] string[] weaponNames;
    [SerializeField] GameObject[] bodies;
    [SerializeField] PlayerData playerData;
    WeaponMenu weaponMenu;
    float ratio;
    int weaponIndex;

    // Start is called before the first frame update
    void Awake()
    {
        weaponMenu = GetComponentInParent<WeaponMenu>();
    }

    private void Start()
    {
        weaponIndex = Utils.GetWeaponIndex(weapon);
        ActivateBody(playerData.equippedElements[weaponIndex]);
    }

    private void SelectWeapon(object sender, System.EventArgs e)
    {
        Vector3 currentPosition = transform.localPosition;
        StopAllCoroutines();
        if(weaponMenu.weaponSelected == weapon)
        {
            StartCoroutine(MoveObject(transform, selectedPosition, currentPosition, weaponMenu.transitionTime));
        }
        else
        {
            StartCoroutine(MoveObject(transform, awayPosition, currentPosition, weaponMenu.transitionTime));
        }
    }

    IEnumerator MoveObject(Transform objectTransform, Vector3 destination, Vector3 currentPosition, float time)
    {
        float timer = time;
        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            ratio = timer / time;
            objectTransform.localPosition = Vector3.Lerp(destination, currentPosition, ratio);
            yield return new WaitForEndOfFrame();
        }
        objectTransform.localPosition = destination;
    }

    void ActivateBody(WeaponElement element)
    {
        ActivateBody(Utils.GetElementIndex(element));
    }

    void ActivateBody(int index)
    {
        weaponName.text = weaponNames[index];
        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i] == null) continue;
            bodies[i].SetActive(index == i);
        }
    }

    private void Instance_onChangeWeapon(object sender, int index)
    {
        if (index != weaponIndex) return;
        ActivateBody(playerData.equippedElements[weaponIndex]);
    }

    private void OnEnable()
    {
        weaponMenu.onSelectWeapon += SelectWeapon;
        GlobalEvents.instance.onChangeWeapon += Instance_onChangeWeapon;
    }

    private void OnDisable()
    {
        weaponMenu.onSelectWeapon -= SelectWeapon;
        GlobalEvents.instance.onChangeWeapon -= Instance_onChangeWeapon;
    }
}
