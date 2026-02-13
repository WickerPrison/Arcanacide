using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] int floor;
    public Color floorColor;
    [SerializeField] Material menuShaderMaterial;
    [SerializeField] Material gradientShaderMaterial;
    [SerializeField] TMP_SpriteAsset[] tmpColorChangeMaterial;
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    PostProcessVolume volume;
    Vignette vignette;
    [SerializeField] float vignetteDuration;
    [SerializeField] float vignetteIntensity;

    //This script makes the camera follow the player. Some delay is added to prevent jerking the camera when the player dashes.

    [SerializeField] GameObject movePoint;
    PlayerMovement playerController;
    Vector3 offset = new Vector3(-0.4f, 7.4f, -7.7f);

    private void Awake()
    {
        menuShaderMaterial.SetColor("_NewColor", floorColor);
        gradientShaderMaterial.SetColor("_NewColor", floorColor);
        for(int i = 0; i < tmpColorChangeMaterial.Length; i++)
        {
            tmpColorChangeMaterial[i].material.SetColor("_NewColor", floorColor);
        }
        mapData.floorColor = floorColor;
        mapData.floor = floor;
    }

    // Start is called before the first frame update
    void Start()
    {
        //break the parent/child relationship of the camera and the movePoint so they can move independently
        movePoint.transform.parent = null;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
        vignette.intensity.overrideState = true;
    }

    // Update is called once per frame
    void Update()
    {
        //the movePoint perfectly copies the movement of the player
        movePoint.transform.position = playerController.transform.position + offset;
        transform.position = movePoint.transform.position;
    }

    private void FixedUpdate()
    {
        //the camera always moves towards the movePoint at a steady speed
        transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, playerController.moveSpeed * 2 * Time.fixedDeltaTime);
    }

    public IEnumerator ScreenShake(float duration, float magnitude)
    {
        float timer = duration;

        while (timer > 0)
        {
            float xPosition = transform.position.x + Random.Range(-1, 1) * magnitude;
            float yPosition = transform.position.y + Random.Range(-1, 1) * magnitude;
            float zPosition = transform.position.z + Random.Range(-1, 1) * magnitude;
            transform.position = new Vector3(xPosition, yPosition, zPosition);

            timer -= Time.unscaledDeltaTime;

            yield return null;
        }
    }

    public IEnumerator NoStamina()
    {
        //float timer = vignetteDuration;
        //while(timer > 0)
        //{
        //    timer -= Time.deltaTime;

        //    vignette.intensity.value = vignetteCurve.Evaluate(1 - timer / vignetteDuration) * vignetteIntensity;

        //    yield return null;
        //}

        float timer = 0.2f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;

            vignette.intensity.value = Mathf.Lerp(0, vignetteIntensity, 1 - timer / 0.2f);

            yield return null;
        }

        vignette.intensity.value = vignetteIntensity;
    }

    public IEnumerator HasStamina()
    {
        float timer = 0.2f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0, 1 - timer / 0.2f);

            yield return null;
        }
        vignette.intensity.value = 0;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onScreenShake += Global_onScreenShake;
        GlobalEvents.instance.onNoStamina += Global_onNoStamina;
        GlobalEvents.instance.onHasStamina += Global_onHasStamina;
    }


    private void OnDisable()
    {
        GlobalEvents.instance.onScreenShake -= Global_onScreenShake;
        GlobalEvents.instance.onNoStamina -= Global_onNoStamina;
        GlobalEvents.instance.onHasStamina -= Global_onHasStamina;
    }

    private void Global_onScreenShake(object sender, (float, float) durationMagnitude)
    {
        StartCoroutine(ScreenShake(durationMagnitude.Item1, durationMagnitude.Item2));
    }

    private void Global_onNoStamina(object sender, System.EventArgs e)
    {
        StartCoroutine(NoStamina());
    }

    private void Global_onHasStamina(object sender, System.EventArgs e)
    {
        StartCoroutine(HasStamina());
    }
}
