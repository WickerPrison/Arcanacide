using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BugReporter : MonoBehaviour
{
    InputManager im;

    string url = "http://localhost:3001/reports";

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.BugReport.Enable();
        im.controls.BugReport.BugReport.performed += ctx => OpenBugReport();
    }

    void OpenBugReport()
    {
        StartCoroutine(CreateIssue("title", "description"));
    }

    IEnumerator GetTest()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);
            }
        }
    }

    IEnumerator CreateIssue(string title, string description)
    {
        WWWForm form = new WWWForm();
        form.AddField("title", title);
        form.AddField("description", description);
        using(UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);
            }
        }
    }
}
