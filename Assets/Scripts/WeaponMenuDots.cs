using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenuDots : MonoBehaviour
{
    [SerializeField] List<Image> dots;
    WeaponScroll weaponScroll;
    [SerializeField] float dotSpacing;
    [SerializeField] MapData mapData;
    Vector3 bigScale = new Vector3(0.14f, 0.14f, 0.14f);
    Vector3 smallScale = new Vector3(0.08f, 0.08f, 0.08f);
    float transitionTime = 0.2f;

    void Awake()
    {
        weaponScroll = GetComponentInParent<WeaponScroll>();
    }

    private void Start()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            int diff = i - weaponScroll.position;
            //dots[i].rectTransform.localPosition = SetXPos(dots[i].rectTransform.localPosition, diff * dotSpacing);
            if (diff == 0)
            {
                dots[i].rectTransform.localScale = bigScale;
                dots[i].color = mapData.floorColor;
            }
            else
            {
                dots[i].rectTransform.localScale = smallScale;
                dots[i].color = Color.white;
            }
        }
    }

    private void WeaponScroll_onScroll(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        for (int i = 0; i < dots.Count; i++)
        {
            int diff = i - weaponScroll.position;
            Vector3 dotPosition = SetXPos(dots[i].rectTransform.localPosition, diff * dotSpacing);

            StartCoroutine(MoveDot(dots[i], dots[i].rectTransform.localPosition, diff == 0));
        }
    }

    public IEnumerator MoveDot(Image dot, Vector3 destination, bool isSelected)
    {
        float timer = transitionTime;
        float ratio;
        Vector3 startPos = dot.rectTransform.localPosition;
        Vector3 startScale = dot.rectTransform.localScale;
        Vector3 endScale = isSelected ? bigScale : smallScale;
        dot.color = Color.white;
        if (isSelected)
        {
            dot.color = mapData.floorColor;
        }
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            ratio = timer / transitionTime;
            dot.rectTransform.localPosition = Vector3.Lerp(destination, startPos, ratio);
            dot.rectTransform.localScale = Vector3.Lerp(endScale, startScale, ratio);
            yield return new WaitForEndOfFrame();
        }
        dot.rectTransform.localPosition = destination;
    }

    Vector3 SetXPos(Vector3 oldVector, float newX)
    {
        return new Vector3(newX, oldVector.y, oldVector.z);
    }

    private void OnEnable()
    {
        weaponScroll.onScroll += WeaponScroll_onScroll;
    }

    private void OnDisable()
    {
        weaponScroll.onScroll -= WeaponScroll_onScroll;
    }
}
