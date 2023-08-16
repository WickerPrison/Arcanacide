using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    public Dictionary<string, string> bindings = new Dictionary<string, string>();
}
