using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    float messageTime = 3;

    public void ShowMessage()
    {
        text.gameObject.SetActive(true);
        StartCoroutine(Message());
    }

    IEnumerator Message()
    {
        yield return new WaitForSeconds(messageTime);
        text.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
