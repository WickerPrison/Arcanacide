using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class RoomSetupScript : MonoBehaviour
{
    public Vector2 roomSize = Vector2.one;
    public event EventHandler onSizeChange;

    private void OnValidate()
    {
        onSizeChange?.Invoke(this, EventArgs.Empty);
    }

    /*    [SerializeField] Transform floor;
        [SerializeField] Transform wall1;
        [SerializeField] Transform wall2;
        [SerializeField] Transform invisibleWall1;
        [SerializeField] Transform invisibleWall2;
        [SerializeField] RawImage floorImage;
        [SerializeField] Vector2 floorScale = Vector2.zero;
        float floorScaleX;
        float floorScaleZ;*/

    /*    private void Start()
        {
            if (floorScale == Vector2.zero)
            {
                floorScaleX = floor.localScale.x;
                floorScaleZ = floor.localScale.z;
            }
            else
            {
                floorScaleX = floorScale.x;
                floorScaleZ = floorScale.y;
            }

            floorImage.uvRect = new Rect(floorImage.uvRect.x, floorImage.uvRect.y, floorScaleX, floorScaleZ);
        }*/

    /*    private void OnDrawGizmosSelected()
        {
            wall1.localPosition = new Vector3(floor.localPosition.x, floor.localPosition.y + 2.5f, floor.localPosition.z + floor.localScale.z * 5);
            wall1.localScale = new Vector3(floor.localScale.x, 0.5f, 0.5f);
            wall2.localPosition = new Vector3(floor.localPosition.x + floor.localScale.x * 5, floor.localPosition.y + 2.5f, floor.localPosition.z);
            wall2.localScale = new Vector3(floor.localScale.z, 0.5f, 0.5f);
            invisibleWall1.localPosition = new Vector3(floor.localPosition.x, floor.localPosition.y + 2.5f, floor.localPosition.z - floor.localScale.z * 5);
            invisibleWall1.localScale = new Vector3(floor.localScale.x, 0.5f, 0.5f);
            invisibleWall2.localPosition = new Vector3(floor.localPosition.x - floor.localScale.x * 5, floor.localPosition.y + 2.5f, floor.localPosition.z);
            invisibleWall2.localScale = new Vector3(floor.localScale.z, 0.5f, 0.5f);

            if(floorScale == Vector2.zero)
            {
                floorScaleX = floor.localScale.x;
                floorScaleZ = floor.localScale.z;
            }
            else
            {
                floorScaleX = floorScale.x;
                floorScaleZ = floorScale.y;
            }



            floorImage.uvRect = new Rect(floorImage.uvRect.x, floorImage.uvRect.y, floorScaleX, floorScaleZ);
        }*/
}
#endif
