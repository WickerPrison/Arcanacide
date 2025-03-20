using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemShard : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] string shardGUID = "";
    Interactable interactable;

    private void Awake()
    {
        if (playerData.gemShards.Contains(shardGUID))
        {
            Destroy(this.gameObject);
        }

        interactable = GetComponent<Interactable>();
        interactable.active = true;
    }

    private void Interactable_onInteracted(object sender, System.EventArgs e)
    {
        interactable.active = false;
        playerData.gemShards.Add(shardGUID);
        playerData.currentGemShards += 1;
        Destroy(gameObject);
    }

    public void GenerateGUID()
    {
        shardGUID = System.Guid.NewGuid().ToString();
    }

    private void OnEnable()
    {
        interactable.onInteracted += Interactable_onInteracted;
    }

    private void OnDisable()
    {
        interactable.onInteracted -= Interactable_onInteracted;
    }
}
