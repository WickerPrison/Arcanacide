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
            if(_iceChunks == null)
            {
                _iceChunks = GetComponentsInChildren<IceChunk>(true);
            }
            return _iceChunks;
        }
        set { _iceChunks = value; }
    }

    HalfGolemVFX[] _halfGolemVFX;
    HalfGolemVFX[] halfGolemVFX
    {
        get
        {
            if(_halfGolemVFX == null)
            {
                _halfGolemVFX = GetComponentsInChildren<HalfGolemVFX>();
            }
            return _halfGolemVFX;
        }
        set { _halfGolemVFX = value; }
    }

    HalfGolemIK[] _halfGolemIKs;
    HalfGolemIK[] halfGolemIKs
    {
        get
        {
            if(_halfGolemIKs == null)
            {
                _halfGolemIKs = GetComponentsInChildren<HalfGolemIK>();
            }
            return _halfGolemIKs;
        }
        set { _halfGolemIKs = value; }
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

    public void ResetValues()
    {
        iceChunks = null;
        halfGolemVFX = null;
        halfGolemIKs = null;
    }
}
