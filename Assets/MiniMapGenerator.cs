using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniMapGenerator : MonoBehaviour
{
    [HideInInspector] public int[,] miniMap;

    public GameObject miniMapContainer;
    public GameObject miniMapCanvas;

    private List<GameObject> miniMapPartsResources = new List<GameObject>();
    public const float gapFix = 0.64f;


    [HideInInspector] public List<GameObject> mmParts = new List<GameObject>();

    private void Start()
    {
        miniMapPartsResources = Resources.LoadAll<GameObject>("UI/Minimap").ToList();
    }

    public void generateMiniMap()
    {
        // Clear Minimap
        foreach (Transform t in miniMapContainer.transform) Destroy(t.gameObject);
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
        Vector3 hallwayPos = new Vector3(hallway.transform.position.x, hallway.transform.position.z, 0) * gapFix;
        Quaternion hallwayRot = Quaternion.Euler(0, 0, hallway.transform.rotation.eulerAngles.y);

        mmParts.Add(Instantiate(miniMapPartsResources.Find(x => x.GetComponent<MiniMapPart>().roomDirection == MiniMapPart.RoomDirMM.Hallway), hallwayPos, hallwayRot, miniMapContainer.transform));
    }

    public void spawnRoom(GameObject room)
    {
        Vector3 roomPos = new Vector3(room.transform.position.x, room.transform.position.z, 0) * gapFix;
        Quaternion roomRot = getRoomRotation(room);
        GameObject minimapP = getRoomPF(room.GetComponent<Room>().roomDirection);


        mmParts.Add(Instantiate(minimapP, roomPos, roomRot, miniMapContainer.transform));
        mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().room = room.GetComponent<Room>();
    }

    public Quaternion getRoomRotation(GameObject room)
    {
        Quaternion q = Quaternion.identity;

        q = Quaternion.Euler(0, 0, room.transform.rotation.eulerAngles.y);

        return q;
    }

    public GameObject getRoomPF(Room.RoomDirection roomDirection)
    {
        switch (roomDirection)
        {
            case Room.RoomDirection.OneDoor:
                return miniMapPartsResources.Find(x => x.GetComponent<MiniMapPart>().roomDirection == MiniMapPart.RoomDirMM.OneDoor);
            case Room.RoomDirection.TwoDoorLinear:
                return miniMapPartsResources.Find(x => x.GetComponent<MiniMapPart>().roomDirection == MiniMapPart.RoomDirMM.TwoDoorLinear);
            case Room.RoomDirection.TwoDoorCurve:
                return miniMapPartsResources.Find(x => x.GetComponent<MiniMapPart>().roomDirection == MiniMapPart.RoomDirMM.TwoDoorCurve);
            case Room.RoomDirection.ThreeDoor:
                return miniMapPartsResources.Find(x => x.GetComponent<MiniMapPart>().roomDirection == MiniMapPart.RoomDirMM.ThreeDoor);
            case Room.RoomDirection.FourDoor:
                return miniMapPartsResources.Find(x => x.GetComponent<MiniMapPart>().roomDirection == MiniMapPart.RoomDirMM.FourDoor);
            default:
                break;
        }

        return miniMapPartsResources[0].gameObject;
    }
}
