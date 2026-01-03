using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponDescriptionBody : MonoBehaviour
{
    [SerializeField] List<RectTransform> sections;
    [SerializeField] PlayerData playerData;
    WeaponScroll weaponScroll;

    void Awake()
    {
        weaponScroll = GetComponentInParent<WeaponScroll>();
    }

    private void Start()
    {
        for(int i = 0; i < sections.Count; i++)
        {
            sections[i].localPosition = new Vector3(i * weaponScroll.bodySpacing - weaponScroll.position * weaponScroll.bodySpacing, 0, 0);
        }

        if (!playerData.unlockedAbilities.Contains(UnlockableAbilities.SPECIAL_ATTACK))
        {
            sections[sections.Count - 1].GetComponent<TextMeshProUGUI>().text = "You have not unlocked Special Attacks.";
        }
    }

    private void WeaponScroll_onScroll(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        for (int i = 0; i < sections.Count; i++)
        {
            Vector3 bodyPosition = new Vector3(i * weaponScroll.bodySpacing - weaponScroll.position * weaponScroll.bodySpacing, 0, 0);
            StartCoroutine(weaponScroll.MoveObject(sections[i], bodyPosition, sections[i].localPosition, weaponScroll.scrollTime));
        }
    }

    public void SetActive(bool active)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
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
