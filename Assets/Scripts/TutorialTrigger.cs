using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] string tutorialName;
    [SerializeField] string nextMessageName;
    [SerializeField] PlayerData playerData;
    TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerData.tutorials.Contains(tutorialName))
        {
            tutorialManager.Tutorial(tutorialName, nextMessageName);
        }
    }
}
