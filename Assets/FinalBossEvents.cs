using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossEvents : MonoBehaviour
{
    public event EventHandler standUp;
    public event EventHandler assistantSitUp;
    public event EventHandler endDialogue;

    public void StandUp()
    {
        standUp?.Invoke(this, EventArgs.Empty);
    }

    public void SitUp()
    {
        assistantSitUp?.Invoke(this, EventArgs.Empty);
    }

    public void EndDialogue()
    {
        endDialogue?.Invoke(this, EventArgs.Empty);
    }
}
