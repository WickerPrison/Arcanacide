using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class HalfGolemToggles : MonoBehaviour
{
    IceChunk[] _iceChunks;
    public IceChunk[] iceChunks
    {
        get
        {
            if(_iceChunks.Length == 0)
            {
                _iceChunks = GetComponentsInChildren<IceChunk>();
            }
            return _iceChunks;
        }
    }

    HalfGolemVFX[] _halfGolemVFX;
    HalfGolemVFX[] halfGolemVFX
    {
        get
        {
            if(_halfGolemVFX.Length == 0)
            {
                _halfGolemVFX = GetComponentsInChildren<HalfGolemVFX>();
            }
            return _halfGolemVFX;
        }
    }

    HalfGolemIK[] _halfGolemIKs;
    HalfGolemIK[] halfGolemIKs
    {
        get
        {
            if(_halfGolemIKs.Length == 0)
            {
                _halfGolemIKs = GetComponentsInChildren<HalfGolemIK>();
            }
            return _halfGolemIKs;
        }
    }

    public void ShowIceChunks(bool showChunks)
    {
        foreach (IceChunk iceChunk in iceChunks)
        {
            iceChunk.gameObject.SetActive(showChunks);
        }
        foreach (HalfGolemVFX vfx in halfGolemVFX)
        {
            vfx.gameObject.SetActive(showChunks);
        }
    }

    public void EnableIK(bool enableIK)
    {
        foreach (HalfGolemIK ik in halfGolemIKs)
        {
            ik.GetComponent<LimbSolver2D>().weight = enableIK ? 1 : 0;
        }
    }
}
