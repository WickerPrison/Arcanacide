using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledByParent : MonoBehaviour
{
    public IKillChildren parent;

    private void OnEnable()
    {
        parent.onKillChildren += Parent_onKillChildren;
    }

    private void OnDisable()
    {
        parent.onKillChildren -= Parent_onKillChildren;
    }

    private void Parent_onKillChildren(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
}
