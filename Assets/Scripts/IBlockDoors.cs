using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBlockDoors
{
    public event EventHandler<bool> onBlockDoor;
}
