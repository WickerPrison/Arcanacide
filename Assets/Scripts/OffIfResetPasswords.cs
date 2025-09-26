using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffIfResetPasswords : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] bool onWhenFinished;

    void Start()
    {
        if (mapData.itWorkerQuestComplete)
        {
            gameObject.SetActive(false);
            return;
        }

        bool isFinished = mapData.resetPasswords.Count == 4;
        gameObject.SetActive(onWhenFinished == isFinished);
    }

}
