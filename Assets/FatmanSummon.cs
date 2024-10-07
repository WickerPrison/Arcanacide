using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatmanSummon : MonoBehaviour, IGetSummoned
{
    FacePlayerSummon facePlayer;
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    AttackArcGenerator attackArcGenerator;
    Vector3 away = new Vector3(100, 100, 100);

    // Start is called before the first frame update
    void Awake()
    {
        facePlayer = GetComponent<FacePlayerSummon>();
        attackArcGenerator = GetComponentInChildren<AttackArcGenerator>();
        transform.position = away;
    }

    public void Attack()
    {
        attackArcGenerator.HideAttackArc();
    }

    public void ShowIndicator()
    {
        attackArcGenerator.ShowAttackArc();
    }

    public void HideIndicator()
    {
        attackArcGenerator.HideAttackArc();
    }

    public void SetDirection(Vector3 direction)
    {
        facePlayer.SetDestination(transform.position + direction);
        facePlayer.ManualFace();
    }

    public void CallAnimation(string animationName)
    {
        frontAnimator.Play(animationName);
        backAnimator.Play(animationName);
    }

    public void GoAway()
    {
        transform.position = away;
    }
}
