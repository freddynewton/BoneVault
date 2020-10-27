using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniMapGenerator : MonoBehaviour
{
    [HideInInspector] public int[,] miniMap;

    public GameObject MiniMapCanvas;

    private List<GameObject> miniMapPartsResources = new List<GameObject>();

    [HideInInspector] public List<GameObject> mmParts = new List<GameObject>();

    private void Start()
    {
        miniMapPartsResources = Resources.LoadAll<GameObject>("UI/Minimap").ToList();
    }

    public void generateMiniMap()
    {
        // Clear Minimap
        foreach (Transform t in gameObject.transform) Destroy(t.gameObject);
        mmParts.Clear();

        // Get World Map
        miniMap = WorldGeneratorManager.Instance.map;

        // Spawn Rooms
        foreach (GameObject room in WorldGeneratorManager.Instance.rooms)
        {
            spawnRoom(room);
        }
    }

    public void spawnRoom(GameObject room)
    {
        switch (room.GetComponent<Room>().roomDirection)
        {
            case Room.RoomDirection.OneDoor:
                mmParts.Add(Instantiate(getRoomPF(Room.RoomDirection.OneDoor), room.transform.position, room.transform.rotation, MiniMapCanvas.transform));
                break;
            case Room.RoomDirection.TwoDoorLinear:
                break;
            case Room.RoomDirection.TwoDoorCurve:
                break;
            case Room.RoomDirection.ThreeDoor:
                break;
            case Room.RoomDirection.FourDoor:
                break;
            default:
                break;
        }
    }

    public GameObject getRoomPF(Room.RoomDirection roomDirection)
    {
        switch (roomDirection)
        {
            case Room.RoomDirection.OneDoor:
                foreach (GameObject g in miniMapPartsResources)
                {

                }
                break;
            case Room.RoomDirection.TwoDoorLinear:
                break;
            case Room.RoomDirection.TwoDoorCurve:
                break;
            case Room.RoomDirection.ThreeDoor:
                break;
            case Room.RoomDirection.FourDoor:
                break;
            default:
                break;
        }

        return miniMapPartsResources[0].gameObject;
    }
}
