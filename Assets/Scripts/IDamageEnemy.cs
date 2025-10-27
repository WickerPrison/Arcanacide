using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageEnemy
{
    public Transform transform { get; }
    public bool blockable { get; set; } 
}
