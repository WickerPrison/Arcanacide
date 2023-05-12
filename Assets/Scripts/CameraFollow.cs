using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Color floorColor;
    [SerializeField] Material menuShaderMaterial;
    [SerializeField] Material gradientShaderMaterial;
    [SerializeField] PlayerData playerData;

    //This script makes the camera follow the player. Some delay is added to prevent jerking the camera when the player dashes.

    [SerializeField] GameObject movePoint;
    PlayerController playerController;
    Vector3 offset = new Vector3(-0.4f, 7.4f, -7.7f);

    private void Awake()
    {
        menuShaderMaterial.SetColor("_NewColor", floorColor);
        gradientShaderMaterial.SetColor("_NewColor", floorColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        //break the parent/child relationship of the camera and the movePoint so they can move independently
        movePoint.transform.parent = null;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
}
