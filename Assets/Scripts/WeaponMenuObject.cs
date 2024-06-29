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
    WeaponMenu weaponMenu;
    float ratio;

    // Start is called before the first frame update
    void Awake()
    {
        weaponMenu = GetComponentInParent<WeaponMenu>();
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

    private void OnEnable()
    {
        weaponMenu.onSelectWeapon += SelectWeapon;
    }

    private void OnDisable()
    {
        weaponMenu.onSelectWeapon -= SelectWeapon;
    }
}
