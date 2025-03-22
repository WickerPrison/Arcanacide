using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerVanquished : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] BuildMode buildMode;

    float messageTime = 3;

    private void onBossKilled(object sender, System.EventArgs e)
    {
        text.gameObject.SetActive(true);
        StartCoroutine(VanquishedMessage());
    }

    IEnumerator VanquishedMessage()
    {
        yield return new WaitForSeconds(messageTime);
        text.gameObject.SetActive(false);
        if(buildMode.buildMode == BuildModes.DEMO) EndOfDemoMessage();
        Destroy(gameObject);
    }

    void EndOfDemoMessage()
    {
        TutorialManager tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
        tutorialManager.Tutorial("End Of Demo");
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onBossKilled += onBossKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onBossKilled -= onBossKilled;
    }
}
