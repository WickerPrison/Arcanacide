using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
public class RoomSetupScript : MonoBehaviour
{
    public event EventHandler onGetInitialVals;
    public Vector2 roomSize = Vector2.one;
    Vector2 roomSizeCache;
    public event EventHandler onSizeChange;
    public FloorTextures floorTexture;
    FloorTextures floorTextureCache;
    public event EventHandler onTextureChange;
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
        onGetInitialVals?.Invoke(this, EventArgs.Empty);
        roomSizeCache = roomSize;
        floorTextureCache = floorTexture;
        floorTilingCache = floorTilingRatio;
        floorTilingOffsetCache = floorTilingOffset;
    }

    private void OnValidate()
    {
        if(roomSize != roomSizeCache)
        {
            onSizeChange?.Invoke(this, EventArgs.Empty);
            UpdateNavmesh();
            roomSizeCache = roomSize;
        }

        if(floorTextureCache != floorTexture)
        {
            onTextureChange?.Invoke(this, EventArgs.Empty);
            floorTextureCache = floorTexture;
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
