using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
public class FloorSetup : MonoBehaviour
{
    RoomSetupScript roomSetup;
    private void OnEnable()
    {
        roomSetup = GetComponentInParent<RoomSetupScript>();
        roomSetup.roomSize = new Vector2(transform.localScale.x, transform.localScale.z);
        roomSetup.onSizeChange += onSizeChange;
    }

    private void onSizeChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(transform, "Resize Room");
        transform.localScale = new Vector3(roomSetup.roomSize.x, 0, roomSetup.roomSize.y);
    }

    private void OnDisable()
    {
        roomSetup.onSizeChange -= onSizeChange;
    }
}
#endif
