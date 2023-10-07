using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class AudioMethods
{
    public static void PlayOneShot(EventReference fmodEvent, string parameter, float value, Vector3 position)
    {
        EventInstance instance = RuntimeManager.CreateInstance(fmodEvent);
        instance.setParameterByName(parameter, value);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }

    public static void PlayOneShot(EventReference fmodEvent, string[] parameters, float[] values, Vector3 position)
    {
        EventInstance instance = RuntimeManager.CreateInstance(fmodEvent);
        for(int i = 0; i < parameters.Length; i++)
        {
            instance.setParameterByName(parameters[i], values[i]);
        }
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }
}
