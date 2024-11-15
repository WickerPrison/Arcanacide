using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

enum ReportMode
{
    BUG, FEEDBACK
}

public class BugReporter : MonoBehaviour
{
    [SerializeField] TMP_InputField titleInput;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] GameObject bugModeUI;
    [SerializeField] TMP_InputField whatHappenedInput;
    [SerializeField] TMP_InputField descriptionInput;
    [SerializeField] GameObject feedbackModeUI;
    [SerializeField] TMP_InputField feedbackInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] Color errorColor;
    [System.NonSerialized] public PauseMenuButtons pauseMenu;
    ReportMode reportMode = ReportMode.BUG;
    //string url = "http://localhost:3001/reports";
    string url = "https://bugreporter-production.up.railway.app/reports";

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        message.text = "";
    }

    public void ToggleMode()
    {
        if (dropdown.value == 0) BugMode();
        else FeedbackMode();
    }

    void BugMode()
    {
        bugModeUI.SetActive(true);
        feedbackModeUI.SetActive(false);
        reportMode = ReportMode.BUG;
    }

    void FeedbackMode()
    {
        bugModeUI.SetActive(false);
        feedbackModeUI.SetActive(true);
        reportMode = ReportMode.FEEDBACK;
    }

    public void CloseBugReport()
    {
        EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton);
        pauseMenu.controls.Enable();
        Destroy(gameObject);
    }

    public void ReportIssue()
    {
        if(titleInput.text == "")
        {
            FillOutError();
            return;
        }
        string description = "";
        if(reportMode == ReportMode.BUG)
        {
            if(whatHappenedInput.text == "" || descriptionInput.text == "")
            {
                FillOutError();
                return;  
            }
            description = "Situation: " + whatHappenedInput.text + "\n\n" + "Bug: " + descriptionInput.text;
        }
        if(reportMode == ReportMode.FEEDBACK)
        {
            if(feedbackInput.text == "")
            {
                FillOutError();
                return;
            }
            description = feedbackInput.text;
        }
        if(emailInput.text != "")
        {
            if(!Regex.IsMatch(emailInput.text, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
            {
                message.color = errorColor;
                message.text = "Invalid email address";
                return;
            }
        }

        message.color = Color.white;
        message.text = "Processing... This may take a few minutes.";
        StartCoroutine(CreateIssue(titleInput.text, description, emailInput.text));
        
    }

    void FillOutError()
    {
        message.color = errorColor;
        message.text = "Please Filll Out All Required Fields";
    }

    IEnumerator CreateIssue(string title, string description, string email = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("title", title);
        form.AddField("description", description);
        if(reportMode == ReportMode.BUG)
        {
            form.AddField("type", "Bug");
        }
        else
        {
            form.AddField("type", "Feedback");
        }
        if (email != null) form.AddField("email", email);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                message.color = errorColor;
                message.text = "Connection Error";
                Debug.LogError(request.error);
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log(response);
                if (response == "Success!")
                {
                    message.color = Color.white;
                    message.text = "Success! Thank you for your feedback!";
                    titleInput.text = "";
                    whatHappenedInput.text = "";
                    descriptionInput.text = "";
                    feedbackInput.text = "";
                    emailInput.text = "";
                }
                else
                {
                    message.color = errorColor;
                    message.text = "An unknown error has occured";
                }
            }
        }
    }


    //IEnumerator GetTest()
    //{
    //    using(UnityWebRequest request = UnityWebRequest.Get(url))
    //    {
    //        yield return request.SendWebRequest();

    //        if(request.result == UnityWebRequest.Result.ConnectionError)
    //        {
    //            Debug.LogError(request.error);
    //        }
    //        else
    //        {
    //            string json = request.downloadHandler.text;
    //            Debug.Log(json);
    //        }
    //    }
    //}
}
