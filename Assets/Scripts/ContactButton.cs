using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContactButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject textingScreenPrefab;
    [SerializeField] Image backgroundImage;
    [SerializeField] GameObject newIcon;
    Material mat;
    public TextingMenu textingMenu;
    public string contactName;
    public bool newMessage;
    public int listIndex;
    public RectTransform content;
    Button button;
    SoundManager sm;
    float fade = 0;

    private void Awake()
    {
        mat = Instantiate(backgroundImage.material);
        backgroundImage.material = mat;
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        SetUpMenuControls();
        text.text = contactName;
        if (!newMessage)
        {
            newIcon.SetActive(false);
        }
    }

    public void OpenTextingScreen()
    {
        sm.ButtonSound();
        TextingScreen textingScreen = Instantiate(textingScreenPrefab).GetComponent<TextingScreen>();
        textingScreen.contactName = contactName;
        textingScreen.textingMenu = textingMenu;
        textingMenu.controls.Disable();
    }

    void SetUpMenuControls()
    {
        button = GetComponent<Button>();
        Navigation nav = button.navigation;
        if (listIndex != 0)
        {
            nav.selectOnUp = textingMenu.contactButtons[listIndex - 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnUp = textingMenu.leaveButton.GetComponent<Button>();
        }

        if (listIndex < textingMenu.contactButtons.Count - 1)
        {
            nav.selectOnDown = textingMenu.contactButtons[listIndex + 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnDown = textingMenu.leaveButton.GetComponent<Button>();
        }

        button.navigation = nav;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(fade, 2f, 0.6f));
        text.color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(fade, 0, .2f));
        text.color = Color.black;
    }

    IEnumerator Fade(float currentFade, float targetFade, float transitionTime)
    {
        float timer = transitionTime;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;
            fade = Mathf.Lerp(targetFade, currentFade, ratio);
            mat.SetFloat("_FadeIn", fade);
            yield return new WaitForEndOfFrame();
        }
    }
}
