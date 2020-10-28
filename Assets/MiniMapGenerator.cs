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
        foreach (Transform t in MiniMapCanvas.transform) Destroy(t.gameObject);
        mmParts.Clear();

        // Get World Map
        miniMap = WorldGeneratorManager.Instance.map;

        // Spawn Rooms
        foreach (GameObject room in WorldGeneratorManager.Instance.rooms)
        {
            spawnRoom(room);
        }

        // Spawn Hallways
        foreach (GameObject hallway in WorldGeneratorManager.Instance.hallWays)
        {
            spawnHallways(hallway);
        }
    }

    public void spawnHallways(GameObject hallway)
    {
        Vector3 hallwayPos = new Vector3(hallway.transform.position.x, hallway.transform.position.z, 0);
        Quaternion hallwayRot = Quaternion.Euler(0, 0, hallway.transform.rotation.eulerAngles.y);

        mmParts.Add(Instantiate(miniMapPartsResources.Find(x => x.GetComponent<MiniMapPart>().roomDirection == MiniMapPart.RoomDirMM.Hallway), hallwayPos, hallwayRot, MiniMapCanvas.transform));
    }

    public void spawnRoom(GameObject room)
    {
        Vector3 roomPos = new Vector3(room.transform.position.x, room.transform.position.z, 0);
        Quaternion roomRot = Quaternion.Euler(0, 0, room.transform.rotation.eulerAngles.y);

        switch (room.GetComponent<Room>().roomDirection)
        {
            case Room.RoomDirection.OneDoor:
                mmParts.Add(Instantiate(getRoomPF(Room.RoomDirection.OneDoor), roomPos, roomRot, MiniMapCanvas.transform));
                mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().room = room.GetComponent<Room>();
                break;
            case Room.RoomDirection.TwoDoorLinear:
                mmParts.Add(Instantiate(getRoomPF(Room.RoomDirection.TwoDoorLinear), roomPos, roomRot, MiniMapCanvas.transform));
                mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().room = room.GetComponent<Room>();
                break;
            case Room.RoomDirection.TwoDoorCurve:
                mmParts.Add(Instantiate(getRoomPF(Room.RoomDirection.TwoDoorCurve), roomPos, roomRot, MiniMapCanvas.transform));
                mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().room = room.GetComponent<Room>();
                break;
            case Room.RoomDirection.ThreeDoor:
                mmParts.Add(Instantiate(getRoomPF(Room.RoomDirection.ThreeDoor), roomPos, roomRot, MiniMapCanvas.transform));
                mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().room = room.GetComponent<Room>();
                break;
            case Room.RoomDirection.FourDoor:
                mmParts.Add(Instantiate(getRoomPF(Room.RoomDirection.FourDoor), roomPos, roomRot, MiniMapCanvas.transform));
                mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().room = room.GetComponent<Room>();
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
                return Resources.Load<GameObject>("UI/Minimap/1Room_MM");
            case Room.RoomDirection.TwoDoorLinear:
                return Resources.Load<GameObject>("UI/Minimap/2Room_Linear_MM");
            case Room.RoomDirection.TwoDoorCurve:
                return Resources.Load<GameObject>("UI/Minimap/2Room_Curve_MM");
            case Room.RoomDirection.ThreeDoor:
                return Resources.Load<GameObject>("UI/Minimap/3Room_MM");
            case Room.RoomDirection.FourDoor:
                return Resources.Load<GameObject>("UI/Minimap/4Room_MM");
            default:
                break;
        }

        return miniMapPartsResources[0].gameObject;
    }
}
