using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem;

public class WeaponScroll : MonoBehaviour
{
    [SerializeField] MenuWeaponSelected weapon;
    [SerializeField] TextMeshProUGUI leftScroll;
    [SerializeField] TextMeshProUGUI rightScroll;
    [SerializeField] List<RectTransform> titles;
    [SerializeField] List<RectTransform> bodies;
    [SerializeField] int titleSpacing = 200;
    [SerializeField] int bodySpacing = 600;
    [SerializeField] PlayerData playerData;
    WeaponMenu weaponMenu;
    InputManager im;
    int position = 0;
    float ratio;
    float smallerScale = 0.7f;
    float scrollTime = 0.5f;
    int maxPosition = 3;

    void Awake()
    {
        weaponMenu = GetComponentInParent<WeaponMenu>();
    }

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Menu.MenuRight.performed += ctx => Scroll(1);
        im.controls.Menu.MenuLeft.performed += ctx => Scroll(-1);


        if (Gamepad.current == null)
        {
            leftScroll.text = "A";
            rightScroll.text = "D";
        }
        else if (Gamepad.current is DualShockGamepad)
        {
            leftScroll.text = "L1";
            rightScroll.text = "R1";
        }
        else
        {
            leftScroll.text = "LB";
            rightScroll.text = "RB";
        }

        for (int i = 0; i < titles.Count; i++)
        {
            titles[i].localPosition = new Vector3(i * titleSpacing - position * titleSpacing, 0, 0);
            bodies[i].localPosition = new Vector3(i * bodySpacing - position * bodySpacing, 0, 0);
            if (i == 0)
            {
                titles[i].localScale = Vector3.one;
            }
            else
            {
                titles[i].localScale = Vector3.one * smallerScale;
            }
        }

        if(!playerData.unlockedAbilities.Contains("Special Attack"))
        {
            bodies[bodies.Count - 1].GetComponent<TextMeshProUGUI>().text = "You have not unlocked Special Attacks.";
        }
    }


    IEnumerator MoveObject(Transform objectTransform, Vector3 destination, Vector3 currentPosition, float time)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            ratio = timer / time;
            objectTransform.localPosition = Vector3.Lerp(destination, currentPosition, ratio);
            yield return new WaitForEndOfFrame();
        }
        objectTransform.localPosition = destination;
    }

    IEnumerator ScaleObject(Transform objectTransform, Vector3 finalScale, Vector3 initialScale, float time)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            ratio = timer / time;
            objectTransform.localScale = Vector3.Lerp(finalScale, initialScale, ratio);
            yield return new WaitForEndOfFrame();
        }
        objectTransform.localScale = finalScale;
    }

    void Scroll(int direction)
    {
        if (weaponMenu.weaponSelected != weapon) return;

        bool canScroll;
        if (direction == -1) canScroll = position > 0;
        else canScroll = position < maxPosition;

        if (canScroll)
        {
            position += direction;
            StopAllCoroutines();
            for (int i = 0; i < titles.Count; i++)
            {
                Vector3 titlePosition = new Vector3(i * titleSpacing - position * titleSpacing, 0, 0);
                StartCoroutine(MoveObject(titles[i], titlePosition, titles[i].localPosition, scrollTime));

                float scale = smallerScale;
                if (position == i) scale = 1;

                StartCoroutine(ScaleObject(titles[i], Vector3.one * scale, titles[i].localScale, scrollTime));

                Vector3 bodyPosition = new Vector3(i * bodySpacing - position * bodySpacing, 0, 0);
                StartCoroutine(MoveObject(bodies[i], bodyPosition, bodies[i].localPosition, scrollTime));
            }
        }
    }
}
