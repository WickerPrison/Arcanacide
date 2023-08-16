using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    public Dictionary<string, string> bindings = new Dictionary<string, string>();

    public void CreateBindingDictionary(string[] keys, string[] values)
    {
        if(keys == null || values == null) return;
        if (keys.Length != values.Length) 
            Debug.Log("Error: Different number of keys and values");

        bindings.Clear();
        if (keys.Length <= 0) return;

        for(int i = 0; i < keys.Length; i++)
        {
            bindings.Add(keys[i], values[i]);
        }
    }
}
