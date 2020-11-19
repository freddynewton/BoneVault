using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniMapGenerator : MonoBehaviour
{
    [HideInInspector] public int[,] miniMap;

    public GameObject miniMapContainer;
    public GameObject miniMapCanvas;
    public GameObject playerMiniMap;
    public GameObject miniMapCamera;
    public GameObject minimapTextureCanvas;

    private List<GameObject> miniMapPartsResources = new List<GameObject>();
    public const float gapFix = 0.38f;

    [HideInInspector] public List<GameObject> mmParts = new List<GameObject>();

    private void LateUpdate()
    {
        // Set Playerpos
        playerMiniMap.transform.position = new Vector3(PlayerController.Instance.transform.position.x, PlayerController.Instance.transform.position.z, 0) * gapFix;
    }

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
        foreach (GameObject room in WorldGeneratorManager.Instance.rooms) spawnRoom(room);

        // Spawn Hallways
        foreach (GameObject hallway in WorldGeneratorManager.Instance.hallWays) spawnHallways(hallway);

        // disable minimap
        //miniMapCanvas.SetActive(false);

        // close minimap
        minimapTextureCanvas.SetActive(false);

        // set Camera Pos
        StartCoroutine(getCameraPos());
    }

    public IEnumerator getCameraPos()
    {
        yield return new WaitForEndOfFrame();

        Vector3 pos = Vector3.zero;

        if (mmParts.Count != 0)
        {
            try
            {
                foreach (GameObject m in mmParts)
                {
                    pos += m.transform.position;
                }
            }
            catch (System.Exception)
            {
                StartCoroutine(getCameraPos());
            }

            try
            {
                miniMapCamera.transform.position = (pos / mmParts.Count) + Vector3.forward * -256f;
            }
            catch (System.Exception)
            {
                StartCoroutine(getCameraPos());
            }
        }
        else
        {
            StartCoroutine(getCameraPos());
        }
    }

    public void spawnHallways(GameObject hallway)
    {
        Vector3 hallwayPos = new Vector3(hallway.transform.position.x, hallway.transform.position.z, 0) * gapFix;
        Quaternion hallwayRot = Quaternion.Euler(0, 0, hallway.transform.rotation.eulerAngles.y);

        mmParts.Add(Instantiate(miniMapPartsResources.Find(x => x.name == "Hallway"), hallwayPos, hallwayRot, miniMapContainer.transform));
        mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().setPart(hallway.GetComponent<Room>());
    }

    public void spawnRoom(GameObject room)
    {
        Vector3 roomPos = new Vector3(room.transform.position.x, room.transform.position.z, 0) * gapFix;
        Quaternion roomRot = getRoomRotation(room);
        GameObject minimapP = getRoomPF(room.GetComponent<Room>().roomDirection);

        mmParts.Add(Instantiate(minimapP, roomPos, roomRot, miniMapContainer.transform));
        mmParts[mmParts.Count - 1].GetComponent<MiniMapPart>().setPart(room.GetComponent<Room>());
    }

    public Quaternion getRoomRotation(GameObject room)
    {
        return Quaternion.Euler(0, 0, -room.transform.rotation.eulerAngles.y); ;
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