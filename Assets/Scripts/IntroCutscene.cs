using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] List<GameObject> panels = new List<GameObject>();
    int panelIndex = 0;
    PlayerControls controls;
    SoundManager sm;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Dialogue.Next.performed += ctx => NextPanel();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        panels[0].SetActive(true);

        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
    }

    void NextPanel()
    {
        sm.ButtonSound();
        if(panelIndex < 3)
        {
            panels[panelIndex].SetActive(false);
            panelIndex += 1;
            panels[panelIndex].SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("ChoosePath");
        }
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
