using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    float timer;
    Func<bool> canInput;
    Action callback;

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (canInput())
            {
                callback();
                ClearBuffer();
            }
        }
    }

    public void Buffer(Func<bool> _canInput, Action _callback)
    {
        if (_canInput())
        {
            _callback();
            ClearBuffer();
            return;
        }

        canInput = _canInput;
        callback = _callback;
        timer = 0.2f;
    }

    void ClearBuffer()
    {
        timer = 0;
        canInput = null;
        callback = null;
    }
}
