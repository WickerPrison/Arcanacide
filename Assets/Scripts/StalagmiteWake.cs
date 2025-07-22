using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteWake : MonoBehaviour
{
    [SerializeField] StalagmiteTrigger[] stalagmiteTriggers;
    [SerializeField] Transform[] startPoints;
    [SerializeField] Transform[] endPoints;
    int[] multipliers = { 1, -1 };
    MinibossDroneController droneController;
    Vector3 chargeStart;
    Vector3 chargeDestination;
    Vector3 wakeDirection;
    [SerializeField] float wakeSpeed;
    enum WakeState
    {
        OFF, CHARGING, POSTCHARGE
    }
    WakeState state = WakeState.OFF;
    float postChargeTime = 10f;
    float postChargeTimer;

    // Start is called before the first frame update
    void Awake()
    {
        droneController = GetComponentInParent<MinibossDroneController>();
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case WakeState.CHARGING:
                for (int i = 0; i < 2; i++)
                {
                    startPoints[i].transform.position += wakeSpeed * Time.fixedDeltaTime * multipliers[i] * wakeDirection;
                    endPoints[i].transform.position = droneController.transform.position;
                }
                break;
            case WakeState.POSTCHARGE:
                for (int i = 0; i < 2; i++)
                {
                    startPoints[i].transform.position += wakeSpeed * Time.fixedDeltaTime * multipliers[i] * wakeDirection;
                    endPoints[i].transform.position += wakeSpeed * Time.fixedDeltaTime * multipliers[i] * wakeDirection;
                }
                postChargeTimer -= Time.fixedDeltaTime;
                if(postChargeTimer < 0)
                {
                    state = WakeState.OFF;
                    stalagmiteTriggers[0].EnableHitbox(false);
                    stalagmiteTriggers[1].EnableHitbox(false);
                }
                break;
        }
    }

    private void OnEnable()
    {
        droneController.onStartCharge += DroneController_onStartCharge;
        droneController.onEndCharge += DroneController_onEndCharge;
    }

    private void OnDisable()
    {
        droneController.onStartCharge -= DroneController_onStartCharge;
        droneController.onEndCharge -= DroneController_onEndCharge;
    }

    private void DroneController_onStartCharge(object sender, (Vector3, Vector3) vals)
    {
        chargeStart = vals.Item1;
        chargeDestination = vals.Item2;
        wakeDirection = Vector3.Cross(chargeDestination - chargeStart, Vector3.up);
        state = WakeState.CHARGING;

        for(int i = 0; i < 2; i++)
        {
            startPoints[i].position = chargeStart;
            endPoints[i].position = droneController.transform.position;
            stalagmiteTriggers[i].UpdatePosition();
            stalagmiteTriggers[i].EnableHitbox(true);
        }
    }

    private void DroneController_onEndCharge(object sender, System.EventArgs e)
    {
        state = WakeState.POSTCHARGE;
        postChargeTimer = postChargeTime;
    }
}
