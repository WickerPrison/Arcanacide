using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[ExecuteAlways]
public class FloorSetup : MonoBehaviour
{
    RoomSetupScript roomSetup;
    RawImage floorImage;
    private void OnEnable()
    {
        floorImage = GetComponentInChildren<RawImage>();
        roomSetup = GetComponentInParent<RoomSetupScript>();
        roomSetup.roomSize = new Vector2(transform.localScale.x, transform.localScale.z);
        roomSetup.floorTilingScale = floorImage.uvRect;
        roomSetup.onSizeChange += onSizeChange;
        roomSetup.onTileChange += OnTileChange;
    }

    private void OnTileChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(floorImage, "Change tile size");
        floorImage.uvRect = roomSetup.floorTilingScale;
    }

    private void onSizeChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(transform, "Resize Room");
        transform.localScale = new Vector3(roomSetup.roomSize.x, 0, roomSetup.roomSize.y);
    }

    private void OnDisable()
    {
        roomSetup.onSizeChange -= onSizeChange;
        roomSetup.onTileChange -= OnTileChange;
    }
}
#endif
