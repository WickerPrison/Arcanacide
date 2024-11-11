using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChaosSummon : MonoBehaviour
{
    [System.NonSerialized] public FacePlayerSummon facePlayer;
    public Animator frontAnimator;
    public Animator backAnimator;

    [System.NonSerialized] public PlayerScript playerScript;
    [System.NonSerialized] public EnemyScript enemyScript;
    [System.NonSerialized] public EnemySound sfx;
    [System.NonSerialized] public EnemyEvents enemyEvents;
    [System.NonSerialized] public Vector3 away = new Vector3(100, 100, 100);

    public virtual void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        facePlayer = GetComponent<FacePlayerSummon>();
        sfx = GetComponentInChildren<EnemySound>();
        enemyEvents = GetComponent<EnemyEvents>();
    }

    public virtual void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        transform.position = away;
    }

    public virtual void Attack()
    {
        enemyEvents.Attack();
    }

    public virtual void ShowIndicator() { }

    public virtual void HideIndicator() { }

    public void SetDirection(Vector3 direction)
    {
        facePlayer.SetDestination(transform.position + direction);
        facePlayer.ManualFace();
    }

    public void CallAnimation(string animationName)
    {
        frontAnimator.Play("Front" + animationName);
        backAnimator.Play("Back" + animationName);
    }

    public virtual void GoAway()
    {
        transform.position = away;
    }
}
