using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EquipEmblem : MonoBehaviour
{
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject check;
    [SerializeField] GameObject box;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    public EmblemMenu emblemMenu;
    Button button;
    public string emblemName;
    public Patches patchName;
    Patch patch;
    SoundManager sm;
    Vector3 maxCheckSize;
    float transitionTime = 0.1f;
    Vector3 boxPosition;
    Vector3 leftPosition;
    Vector3 rightPosition;
    float vibrateAmp = 2;
    float vibrateDuration = 0.3f;
    float vibrateFreq = 40;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        SetUpMenuControls();
        patch = emblemLibrary.patchDictionary[patchName];
        nameText.text = patch.name;
        descriptionText.text = patch.description;
        maxCheckSize = check.transform.localScale;
        if (!playerData.equippedPatches.Contains(patchName))
        {
            check.transform.localScale = Vector3.zero;
        }
        boxPosition = box.transform.localPosition;
        leftPosition = boxPosition - Vector3.right * vibrateAmp;
        rightPosition = boxPosition + Vector3.right * vibrateAmp;
    }

    public void ButtonPressed()
    {
        sm.ButtonSound();
        if (playerData.equippedPatches.Contains(patchName))
        {
            EmblemUnequip();
        }
        else if(playerData.equippedPatches.Count < playerData.maxPatches)
        { 
            EmblemEquip();
        }
        else
        {
            StartCoroutine(VibrateBox());
        }
    }

    void EmblemEquip()
    {
        playerData.equippedPatches.Add(patchName);
        emblemMenu.UpdatePatchCount();
        StopAllCoroutines();
        box.transform.localPosition = boxPosition;
        StartCoroutine(ScaleAnimation(check.transform.localScale, maxCheckSize));
    }

    void EmblemUnequip()
    {
        playerData.equippedPatches.Remove(patchName);
        emblemMenu.UpdatePatchCount();
        StopAllCoroutines();
        box.transform.localPosition = boxPosition;
        StartCoroutine(ScaleAnimation(check.transform.localScale, Vector3.zero));
    }

    IEnumerator ScaleAnimation(Vector3 currentScale, Vector3 targetScale)
    {
        float timer = transitionTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float ratio = timer / transitionTime;

            check.transform.localScale = Vector3.Lerp(targetScale, currentScale, ratio);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator VibrateBox()
    {
        float timer = vibrateDuration;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float input = Mathf.Cos(timer * vibrateFreq) * 0.5f + 0.5f;
            box.transform.localPosition = Vector3.Lerp(leftPosition, rightPosition, input);
            yield return new WaitForEndOfFrame();
        }
        box.transform.localPosition = boxPosition;
    }

    void SetUpMenuControls()
    {
        button = GetComponent<Button>();
        int listIndex = playerData.patches.IndexOf(patchName);
        Navigation nav = button.navigation;
        if (listIndex != 0)
        {
            nav.selectOnUp = emblemMenu.buttons[listIndex - 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnUp = emblemMenu.leaveButton.GetComponent<Button>();
        }

        if(listIndex < emblemMenu.buttons.Count - 1)
        {
            nav.selectOnDown = emblemMenu.buttons[listIndex + 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnDown = emblemMenu.leaveButton.GetComponent<Button>();
        }

        button.navigation = nav;
    }

    public void SelectOnMouseOver()
    {
        if(Gamepad.current == null)
        {
            button.Select();
        }
    }
}
