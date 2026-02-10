using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FmodUtils
{
    public static void PlayOneShot(EventReference eventReference, float volume, Vector3 position = new Vector3())
    {
        try
        {
            PlayOneShot(eventReference.Guid, volume, position);
        }
        catch (EventNotFoundException)
        {
            RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + eventReference);
        }
    }

    public static void PlayOneShot(string path, float volume, Vector3 position = new Vector3())
    {
        try
        {
            PlayOneShot(RuntimeManager.PathToGUID(path), volume, position);
        }
        catch (EventNotFoundException)
        {
            RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + path);
        }
    }

    public static void PlayOneShot(FMOD.GUID guid, float volume, Vector3 position = new Vector3())
    {
        var instance = RuntimeManager.CreateInstance(guid);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.setVolume(volume);
        instance.start();
        instance.release();
    }
}
