using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPhysicsObject : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;
    }
}
