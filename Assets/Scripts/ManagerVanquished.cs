using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerVanquished : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI healthGemMessage;

    float messageTime = 3;

    public void ShowMessage()
    {
        text.gameObject.SetActive(true);
        StartCoroutine(VanquishedMessage());
    }

    IEnumerator VanquishedMessage()
    {
        yield return new WaitForSeconds(messageTime);
        text.gameObject.SetActive(false);
        healthGemMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageTime);
        EndOfDemoMessage();
        Destroy(gameObject);
    }

    void EndOfDemoMessage()
    {
        TutorialManager tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
        tutorialManager.EndOfDemoTutorial();
    }
}
