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
    public Vector2 floorTilingOffset;
    Vector2 floorTilingOffsetCache;
    public Vector2 floorTilingRatio;
    Vector2 floorTilingCache;
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
        floorTilingCache = floorTilingRatio;
    }

    private void OnValidate()
    {
        if(roomSize != roomSizeCache)
        {
            onSizeChange?.Invoke(this, EventArgs.Empty);
            UpdateNavmesh();
            roomSizeCache = roomSize;
        }

        if(floorTilingCache != floorTilingRatio || floorTilingOffsetCache != floorTilingOffset)
        {
            onTileChange?.Invoke(this, EventArgs.Empty);
            floorTilingCache = floorTilingRatio;
            floorTilingOffsetCache = floorTilingOffset;
        }

        if(!usesNavmesh && NavMesh.navMeshData != null) 
        {
            ClearNavmesh();
        }
    }

    public void ManualUpdate()
    {
        onSizeChange?.Invoke(this, EventArgs.Empty);
        UpdateNavmesh();
        roomSizeCache = roomSize;
        floorTilingCache = floorTilingRatio;
        floorTilingOffsetCache = floorTilingOffset;
    }

    public void UpdateNavmesh()
    {
        if (!usesNavmesh || gameObject.scene.name == null) return;
        NavMesh.BuildNavMesh();
        NavMesh.UpdateNavMesh(NavMesh.navMeshData);
    }

    public void ClearNavmesh()
    {
        NavMesh.RemoveData();
    }
}
#endif
