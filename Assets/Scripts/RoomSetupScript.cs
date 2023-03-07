using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSetupScript : MonoBehaviour
{
    [SerializeField] Transform floor;
    [SerializeField] Transform wall1;
    [SerializeField] Transform wall2;
    [SerializeField] Transform invisibleWall1;
    [SerializeField] Transform invisibleWall2;
    [SerializeField] RawImage floorImage;

    private void Start()
    {
        floorImage.uvRect = new Rect(floorImage.uvRect.x, floorImage.uvRect.y, floor.localScale.x, floor.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        wall1.localPosition = new Vector3(floor.localPosition.x, floor.localPosition.y + 2.5f, floor.localPosition.z + floor.localScale.z * 5);
        wall1.localScale = new Vector3(floor.localScale.x, 0.5f, 0.5f);
        wall2.localPosition = new Vector3(floor.localPosition.x + floor.localScale.x * 5, floor.localPosition.y + 2.5f, floor.localPosition.z);
        wall2.localScale = new Vector3(floor.localScale.z, 0.5f, 0.5f);
        invisibleWall1.localPosition = new Vector3(floor.localPosition.x, floor.localPosition.y + 2.5f, floor.localPosition.z - floor.localScale.z * 5);
        invisibleWall1.localScale = new Vector3(floor.localScale.x, 0.5f, 0.5f);
        invisibleWall2.localPosition = new Vector3(floor.localPosition.x - floor.localScale.x * 5, floor.localPosition.y + 2.5f, floor.localPosition.z);
        invisibleWall2.localScale = new Vector3(floor.localScale.z, 0.5f, 0.5f);

        floorImage.uvRect = new Rect(floorImage.uvRect.x, floorImage.uvRect.y, floor.localScale.x, floor.localScale.z);
    }
}
