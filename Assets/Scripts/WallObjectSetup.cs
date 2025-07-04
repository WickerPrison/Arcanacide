using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class WallObjectSetup : MonoBehaviour
{
    [SerializeField] ObjectDirection direction;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Transform[] messages;
    enum ObjectDirection
    {
        LEFT, RIGHT, UP, DOWN
    }
    RoomSetupScript roomSetup;
    SortingGroup sortingGroup;
#if UNITY_EDITOR

    private void OnEnable()
    {
        roomSetup = FindObjectOfType<RoomSetupScript>();
        roomSetup.onSizeChange += OnSizeChange;
    }

    private void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    private void OnSizeChange(object sender, System.EventArgs e)
    {
        if (roomSetup == null) return;
        UpdateObject();
    }

    private void OnValidate()
    {
        if (roomSetup == null) return;
        UpdateObject();
    }

    private void UpdateObject()
    {
        Undo.RecordObject(this, "Move Wall Object");
        switch (direction)
        {
            case ObjectDirection.LEFT:
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y,
                    roomSetup.roomSize.y * 5);
                transform.localEulerAngles = Vector3.zero;
                SetLayer("Floor");
                PositionMessages(1);
                break;
            case ObjectDirection.RIGHT:
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y,
                    -roomSetup.roomSize.y * 5);
                transform.localEulerAngles = Vector3.zero;
                SetLayer("Foreground");
                PositionMessages(1);
                break;
            case ObjectDirection.UP:
                transform.localPosition = new Vector3(
                    roomSetup.roomSize.x * 5,
                    transform.localPosition.y,
                    transform.localPosition.z);
                transform.localEulerAngles = new Vector3(0, 90, 0);
                SetLayer("Floor");
                PositionMessages(-1);
                break;
            case ObjectDirection.DOWN:
                transform.localPosition = new Vector3(
                    -roomSetup.roomSize.x * 5,
                    transform.localPosition.y,
                    transform.localPosition.z);
                transform.localEulerAngles = new Vector3(0, 90, 0);
                SetLayer("Foreground");
                PositionMessages(-1);
                break;
        }
        if(spriteRenderers.Length > 0)
        {
            foreach(SpriteRenderer renderer in spriteRenderers)
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(renderer);
            }
        }
        if (sortingGroup != null) PrefabUtility.RecordPrefabInstancePropertyModifications(sortingGroup);
        if(messages.Length > 0)
        {
            foreach(Transform message in messages)
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(message);
            }
        }
    }

    private void SetLayer(string layerName)
    {
        if (spriteRenderers.Length > 0)
        {
            Undo.RecordObjects(spriteRenderers, "Move Wall Object");
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.sortingLayerName = layerName;
            }
        }

        if(sortingGroup != null)
        {
            Undo.RecordObject(sortingGroup, "Move Wall Object");
            sortingGroup.sortingLayerName = layerName;
        }
    }

    private void PositionMessages(int direction)
    {
        if (messages.Length <= 0) return;
        foreach(Transform message in messages)
        {
            message.localRotation = Quaternion.Euler(30, direction * 45, message.localEulerAngles.z);
        }
    }

    private void OnDisable()
    {
        if(roomSetup != null)
        {
            roomSetup.onSizeChange -= OnSizeChange;
        }
    }
#endif
}
