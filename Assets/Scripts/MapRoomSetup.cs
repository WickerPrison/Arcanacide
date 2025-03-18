using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]
public class MapRoomSetup : MonoBehaviour
{
    [SerializeField] Vector2 size;
    public List<MapDoorSetup> doors = new List<MapDoorSetup>();
    float scaleFactor = 100;
    bool hasChanged = false;
    RectTransform square;
    RectTransform outline;
#if UNITY_EDITOR

    private void OnEnable()
    {
        square = GetComponent<RectTransform>();
        outline = transform.Find("Outline").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (hasChanged)
        {
            transform.localScale = Vector3.one;
            square.sizeDelta = size * scaleFactor;
            outline.sizeDelta = size * scaleFactor;
            foreach (MapDoorSetup door in doors) SetupDoor(door);
            hasChanged = false;
        }
    }

    private void OnValidate()
    {
        hasChanged = true;
    }

    void SetupDoor(MapDoorSetup door)
    {
        float position;
        switch (door.direction)
        {
            case MapDoorDirection.UP:
                position = size.y * scaleFactor / 2 - 4;
                door.transform.localPosition = new Vector3(door.offset * scaleFactor, position, 0);
                door.transform.localRotation = Quaternion.identity;
                break;
            case MapDoorDirection.DOWN:
                position = size.y * scaleFactor / 2 - 4;
                door.transform.localPosition = new Vector3(door.offset * scaleFactor, -position, 0);
                door.transform.localRotation = Quaternion.identity;
                break;
            case MapDoorDirection.LEFT:
                position = size.x * scaleFactor / 2 - 4;
                door.transform.localPosition = new Vector3(-position, door.offset * scaleFactor, 0);
                door.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case MapDoorDirection.RIGHT:
                position = size.x * scaleFactor / 2 - 4;
                door.transform.localPosition = new Vector3(position, door.offset * scaleFactor, 0);
                door.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }
#endif
}
