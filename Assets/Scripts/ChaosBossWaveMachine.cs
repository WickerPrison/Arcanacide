using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosBossWaveMachine : WaveMachine
{
    [SerializeField] ChaosBossController bossController;
    [SerializeField] int waveMachineId;

    public override void Update()
    {
        
    }

    private void OnEnable()
    {
        bossController.onFireWaves += BossController_onFireWaves;
    }

    private void OnDisable()
    {
        bossController.onFireWaves -= BossController_onFireWaves;
    }

    private void BossController_onFireWaves(object sender, int triggerId)
    {
        if(triggerId == 0 || triggerId == waveMachineId)
        {
            FireWave();
        }
    }
}
