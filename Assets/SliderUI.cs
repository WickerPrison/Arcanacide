using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image inner;
    [SerializeField] Image outer;
    [SerializeField] Image bar;
    bool selected = false;
    PlayerControls controls;
    Vector2 moveDir;
    [SerializeField] float slideAmp;
    Vector2 centerPos;
    Vector2 leftPos;
    Vector2 rightPos;
    float slideSpeed = 0.75f;
    [System.NonSerialized] public float slidePosNorm;
    TextMeshProUGUI buttonText;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.ControllerDirection.performed += ctx => moveDir = ctx.ReadValue<Vector2>();
        controls.Menu.ControllerDirection.canceled += ctx => moveDir = Vector2.zero;
    }

    public virtual void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        centerPos = transform.position;
        leftPos = centerPos - new Vector2(slideAmp, 0);
        rightPos = centerPos + new Vector2(slideAmp, 0);
    }

    private void Update()
    {
        if (!selected) return;

        if(moveDir.x != 0)
        {
            MoveSlider();
        }
    }

    public virtual void MoveSlider()
    {
        slidePosNorm = Mathf.Clamp(slidePosNorm + moveDir.x * Time.unscaledDeltaTime * slideSpeed, 0, 1);
        transform.position = Vector2.Lerp(leftPos, rightPos, slidePosNorm);
        bar.transform.localScale = new Vector3(slidePosNorm, bar.transform.localScale.y, bar.transform.localScale.z);
        buttonText.text = Mathf.RoundToInt(slidePosNorm * 100).ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        inner.color = Color.black;
        outer.color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
        inner.color = Color.white;
        outer.color = Color.black; ;
    }

    private void OnEnable()
    {
        controls.Enable();
    }


    private void OnDisable()
    {
        controls.Disable();
    }
}
