using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FloorSetup : MonoBehaviour
{
#if UNITY_EDITOR
    RoomSetupScript roomSetup;
    RawImage floorImage;
    private void OnEnable()
    {
        floorImage = GetComponentInChildren<RawImage>();
        roomSetup = GetComponentInParent<RoomSetupScript>();
        roomSetup.roomSize = new Vector2(transform.localScale.x, transform.localScale.z);
        roomSetup.floorTilingOffset = new Vector2(floorImage.uvRect.x, floorImage.uvRect.y);
        roomSetup.floorTilingRatio = new Vector2(floorImage.uvRect.width / roomSetup.roomSize.x, floorImage.uvRect.height / roomSetup.roomSize.y);
        roomSetup.onSizeChange += onSizeChange;
        roomSetup.onTileChange += OnTileChange;
    }

    private void OnTileChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(floorImage, "Change tile size");
        UpdateTileSize();
    }

    private void onSizeChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(transform, "Resize Room");
        transform.localScale = new Vector3(roomSetup.roomSize.x, 0, roomSetup.roomSize.y);
        UpdateTileSize();
    }

    private void UpdateTileSize()
    {
        floorImage.uvRect = new Rect(roomSetup.floorTilingOffset.x, roomSetup.floorTilingOffset.y, roomSetup.roomSize.x * roomSetup.floorTilingRatio.x, roomSetup.roomSize.y * roomSetup.floorTilingRatio.y);
    }

    private void OnDisable()
    {
        roomSetup.onSizeChange -= onSizeChange;
        roomSetup.onTileChange -= OnTileChange;
    }
#endif
}
