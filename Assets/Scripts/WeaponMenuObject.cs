using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMenuObject : MonoBehaviour
{
    [SerializeField] MenuWeaponSelected weapon;
    [SerializeField] Vector3 awayPosition;
    [SerializeField] Vector3 selectedPosition;
    WeaponMenu weaponMenu;
    float timer;
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
            StartCoroutine(MoveObject(selectedPosition, currentPosition));
        }
        else
        {
            StartCoroutine(MoveObject(awayPosition, currentPosition));
        }
    }

    IEnumerator MoveObject(Vector3 destination, Vector3 currentPosition)
    {
        timer = weaponMenu.transitionTime;
        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            ratio = timer / weaponMenu.transitionTime;
            transform.localPosition = Vector3.Lerp(destination, currentPosition, ratio);
            yield return new WaitForEndOfFrame();
        }
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
