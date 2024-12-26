using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

#if UNITY_EDITOR
public class RoomSetupScript : MonoBehaviour
{
    public Vector2 roomSize = Vector2.one;
    Vector2 roomSizeCache;
    public event EventHandler onSizeChange;
    public Rect floorTilingScale;
    Rect floorTilingCache;
    public bool usesNavmesh = true;
    public event EventHandler onTileChange;
    private NavMeshSurface navMesh;
    public NavMeshSurface NavMesh
    {
        get
        {
            if(navMesh == null)
            {
                navMesh = GetComponentInChildren<NavMeshSurface>();
            }
            return navMesh;
        }
    }


    private void OnEnable()
    {
        roomSizeCache = roomSize;
        floorTilingCache = floorTilingScale;
    }

    private void OnValidate()
    {
        if(roomSize != roomSizeCache)
        {
            onSizeChange?.Invoke(this, EventArgs.Empty);
            UpdateNavmesh();
            roomSizeCache = roomSize;
        }

        if(floorTilingCache != floorTilingScale)
        {
            onTileChange?.Invoke(this, EventArgs.Empty);
            floorTilingCache = floorTilingScale;
        }

        if(!usesNavmesh && NavMesh.navMeshData != null) 
        {
            ClearNavmesh();
        }
    }

    public void UpdateNavmesh()
    {
        if (!usesNavmesh) return;
        NavMesh.AddData();
        NavMesh.UpdateNavMesh(NavMesh.navMeshData);
    }

    public void ClearNavmesh()
    {
        NavMesh.RemoveData();
    }
}
#endif
