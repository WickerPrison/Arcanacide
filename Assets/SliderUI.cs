using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image inner;
    [SerializeField] Image outer;
    [SerializeField] Image bar;
    bool selected = false;
    PlayerControls controls;
    Vector2 moveDir;
    [SerializeField] Vector2 maxPos;
    [SerializeField] Vector2 minPos;
    float slideSpeed = 0.75f;
    float slideAmp;
    [System.NonSerialized] public float slidePosNorm;
    TextMeshProUGUI buttonText;
    float localToWorld;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.ControllerDirection.performed += ctx => moveDir = ctx.ReadValue<Vector2>();
        controls.Menu.ControllerDirection.canceled += ctx => moveDir = Vector2.zero;
    }

    public virtual void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        slideAmp = Vector2.Distance(minPos, maxPos);
        localToWorld = transform.position.x - transform.localPosition.x;
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
        transform.localPosition = Vector2.Lerp(minPos, maxPos, slidePosNorm);
        bar.transform.localScale = new Vector3(slidePosNorm, bar.transform.localScale.y, bar.transform.localScale.z);
        buttonText.text = Mathf.RoundToInt(slidePosNorm * 100).ToString();
    }

    public void DragSlider()
    {
        float distance = Mouse.current.position.ReadValue().x - (minPos.x + localToWorld);
        slidePosNorm = Mathf.Clamp(distance / slideAmp, 0, 1);
        Debug.Log(transform.position);
        transform.localPosition = Vector2.Lerp(minPos, maxPos, slidePosNorm);
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
        outer.color = Color.black;
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
