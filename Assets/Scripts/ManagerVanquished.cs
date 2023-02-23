using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerVanquished : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    float messageTime = 0;
    bool hasActivated = false;

    public void ShowMessage()
    {
        text.gameObject.SetActive(true);
        messageTime = 3;
        hasActivated = true;
    }

    private void Update()
    {
        if (messageTime > 0)
        {
            messageTime -= Time.deltaTime;
        }
        else if (hasActivated)
        {
            //EndOfDemoMessage();
            Destroy(gameObject);
        }
    }

    void EndOfDemoMessage()
    {
        TutorialManager tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
        tutorialManager.EndOfDemoTutorial();
    }
}
