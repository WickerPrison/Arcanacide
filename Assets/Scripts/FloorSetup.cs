using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public enum FloorTextures
{
    DEFAULT, WAVES, CARPET, SQUARES
}

[ExecuteAlways]
public class FloorSetup : MonoBehaviour
{
    [SerializeField] Texture[] floorTextures;
    Dictionary<FloorTextures, Texture> floorMatDict = new Dictionary<FloorTextures, Texture>();
    Dictionary<Texture, FloorTextures> reverseDict = new Dictionary<Texture, FloorTextures>();
    RoomSetupScript roomSetup;
    RawImage floorImage;
    private void OnEnable()
    {
        floorImage = GetComponentInChildren<RawImage>();
        roomSetup = GetComponentInParent<RoomSetupScript>();
        if (roomSetup == null) return;
        DictSetup();

        roomSetup.onGetInitialVals += RoomSetup_onGetInitialVals;
        roomSetup.onSizeChange += onSizeChange;
        roomSetup.onTextureChange += OnTextureChange;
        roomSetup.onTileChange += OnTileChange;
    }

    private void RoomSetup_onGetInitialVals(object sender, System.EventArgs e)
    {
        roomSetup.roomSize = new Vector2(transform.localScale.x, transform.localScale.z);
        roomSetup.floorTexture = reverseDict[floorImage.texture];
        roomSetup.floorTilingOffset = new Vector2(floorImage.uvRect.x, floorImage.uvRect.y);
        roomSetup.floorTilingRatio = new Vector2(floorImage.uvRect.width / roomSetup.roomSize.x, floorImage.uvRect.height / roomSetup.roomSize.y);
    }

    private void onSizeChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(transform, "Resize Room");
        transform.localScale = new Vector3(roomSetup.roomSize.x, 0, roomSetup.roomSize.y);
        UpdateTileSize();
        PrefabUtility.RecordPrefabInstancePropertyModifications(floorImage);
    }

    private void OnTextureChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(floorImage, "Change floor texture");
        floorImage.texture = floorMatDict[roomSetup.floorTexture];
        PrefabUtility.RecordPrefabInstancePropertyModifications(floorImage);
    }

    private void OnTileChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(floorImage, "Change tile size");
        UpdateTileSize();
        PrefabUtility.RecordPrefabInstancePropertyModifications(floorImage);
    }
    private void UpdateTileSize()
    {
        floorImage.uvRect = new Rect(roomSetup.floorTilingOffset.x, roomSetup.floorTilingOffset.y, roomSetup.roomSize.x * roomSetup.floorTilingRatio.x, roomSetup.roomSize.y * roomSetup.floorTilingRatio.y);
    }

    void DictSetup()
    {
        if (floorMatDict.Keys.Count > 0) return;
        floorMatDict.Add(FloorTextures.DEFAULT, floorTextures[0]);
        floorMatDict.Add(FloorTextures.WAVES, floorTextures[1]);
        floorMatDict.Add(FloorTextures.CARPET, floorTextures[2]);
        floorMatDict.Add(FloorTextures.SQUARES, floorTextures[3]);

        reverseDict.Add(floorTextures[0], FloorTextures.DEFAULT);
        reverseDict.Add(floorTextures[1], FloorTextures.WAVES);
        reverseDict.Add(floorTextures[2], FloorTextures.CARPET);
        reverseDict.Add(floorTextures[3], FloorTextures.SQUARES);

    }

    private void OnDisable()
    {
        if (roomSetup == null) return;
        roomSetup.onGetInitialVals -= RoomSetup_onGetInitialVals;
        roomSetup.onSizeChange -= onSizeChange;
        roomSetup.onTileChange -= OnTileChange;
    }
}
#endif
