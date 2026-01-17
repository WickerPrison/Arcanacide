using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestUtils
{
    public static bool RoughEquals(float val1, float val2, float margin = 0.05f)
    {
        return Mathf.Abs(val1 - val2) < val1 * margin;
    }
}
