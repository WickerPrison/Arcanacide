using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DoorUnlockedMessage : MonoBehaviour
{
    [System.NonSerialized] public int lockId;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI text;
    bool instantiatedCorrectly = false;
    Vector3 startPos;
    float time = 1f;
    float timer;

    public static DoorUnlockedMessage Instantiate(GameObject prefab, int id)
    {
        DoorUnlockedMessage message = Instantiate(prefab).GetComponent<DoorUnlockedMessage>();
        message.lockId = id;
        message.instantiatedCorrectly = true;
        return message;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        canvas.enabled = false;
    }

    private void Start()
    {
        if (!instantiatedCorrectly) Utils.IncorrectInitialization("DoorUnlockMessage");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Doorway[] doorways = GameObject.FindObjectsOfType<Doorway>();
        foreach (Doorway door in doorways)
        {
            if (door.lockedDoorID == lockId)
            {
                transform.position = door.transform.position + Vector3.up * 1f;
                startPos = transform.position;
                StartCoroutine(Message());
                return;
            }
        }
    }

    IEnumerator Message()
    {
        canvas.enabled = true;
        Vector3 endPos = startPos + Vector3.up * 0.8f;
        timer = time;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float ratio = 1 - timer / time;
            text.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), ratio);
            transform.position = Vector3.Slerp(startPos, endPos, ratio);

            yield return null;
        }

        Destroy(gameObject);
    }
}
