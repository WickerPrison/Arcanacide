using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] int floor;
    public Color floorColor;
    [SerializeField] Material menuShaderMaterial;
    [SerializeField] Material gradientShaderMaterial;
    [SerializeField] TMP_SpriteAsset[] tmpColorChangeMaterial;
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;

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

    private void OnEnable()
    {
        GlobalEvents.instance.onScreenShake += Global_onScreenShake;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onScreenShake -= Global_onScreenShake;
    }

    private void Global_onScreenShake(object sender, (float, float) durationMagnitude)
    {
        StartCoroutine(ScreenShake(durationMagnitude.Item1, durationMagnitude.Item2));
    }
}
