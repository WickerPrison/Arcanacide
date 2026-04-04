using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSniperSummon : ChaosSummon
{
    [SerializeField] BeamVfx beamVfx;
    Vector3 initialPosition;
    float crystalFloatPos;
    Vector3 offset = new Vector3(0, 1.8f, 0);
    [SerializeField] bool changeColor = false;
    [SerializeField] GameObject projectilePrefab;
    bool isSummoned = false;
    [System.NonSerialized] public ChaosBossController bossController;
    [SerializeField] SpriteEffects spriteEffects;
    WaitForSeconds dissolveDelay = new WaitForSeconds(0.2f);

    private void Update()
    {
        if (!isSummoned) return;
        ShowBeam();
    }

    public void GetSummoned(int whichSide, Vector3 direction)
    {
        Vector3 perp = Vector3.Cross(direction, Vector3.up).normalized;
        transform.position = enemyScript.transform.position + direction * 3f + perp * 4f * whichSide;
        StartCoroutine(DissolveIn());
    }

    IEnumerator DissolveIn()
    {
        yield return spriteEffects.UnDissolve(0.25f);
        isSummoned = true;
    }

    void ShowBeam()
    {
        RaycastHit hit = beamVfx.BeamHitscan(transform.position, playerScript.transform.position);
        initialPosition = transform.position + Vector3.up * 1.45f + Vector3.up * crystalFloatPos;
        initialPosition += (playerScript.transform.position - transform.position).normalized * .6f;

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            beamVfx.SetPositions(initialPosition, playerScript.transform.position + offset);
            beamVfx.DecrementAimValue(Shoot);
        }
        else
        {
            beamVfx.SetPositions(initialPosition, hit.point + offset);
            beamVfx.ResetAimValue();
        }
    }

    void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = transform.position + offset;
        projectile.direction = playerScript.transform.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.forward, projectile.direction, Vector3.up);
        projectile.transform.rotation = Quaternion.Euler(25, 0, -angle);
        if (changeColor)
        {
            SpriteColorChange spriteColorChange = projectile.GetComponent<SpriteColorChange>();
            spriteColorChange.colorChange = true;
            ParticleColorChange particleColorChange = projectile.GetComponent<ParticleColorChange>();
            particleColorChange.colorChange = true;
        }
        beamVfx.HideBeam();
        StartCoroutine(DoneShooting());
    }

    IEnumerator DoneShooting()
    {
        isSummoned = false;
        yield return dissolveDelay;
        yield return spriteEffects.Dissolve(0.25f);
        GoAway();
    }

    public override void GoAway()
    {
        bossController.snipers.Enqueue(this);
        base.GoAway();
    }
}
