using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    PlayerControls controls;
    int screenshotNumber = 0;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Screenshot.performed += ctx => ScreenShot();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void ScreenShot()
    {
        string filename = "Screenshot" + screenshotNumber.ToString() + ".png";
        ScreenCapture.CaptureScreenshot(filename);
        screenshotNumber += 1;
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
