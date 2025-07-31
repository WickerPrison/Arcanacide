using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string tutorialName;
    [SerializeField] string nextMessageName;
    public PlayerData playerData;
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
