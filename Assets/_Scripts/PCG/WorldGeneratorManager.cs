using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The world get a abstract view as a 2D Integer Array
/// 
/// 0 = Empty
/// 1 = Room
/// 
/// </summary>
public class WorldGeneratorManager : MonoBehaviour
{
    public enum Direction
    {
        Top,
        Left,
        Right,
        Down
    }

    public static WorldGeneratorManager Instance { get; private set; }

    [Header("World Generation Settings")]
    public bool startOnAwake;

    [Header("World Size Settings")]
    public int roomCount;
    public int worldWidth = 10;
    public int worldLength = 10;

    [Header("Placement Settings")]
    public const float roomSpace = 96;

    [HideInInspector] public int[,] map;
    [HideInInspector] public List<Vector2Int> mapSetPos;

    private List<GameObject> rooms = new List<GameObject>();

    private List<GameObject> resourcesEnemyRoomList = new List<GameObject>();
    private List<GameObject> resourcesStartRoomList = new List<GameObject>();
    private List<GameObject> resourcesHallwayList = new List<GameObject>();

    public IEnumerator generateMap()
    {
        // Delete old map
        foreach (Transform child in gameObject.transform) Destroy(child.gameObject);
        rooms.Clear();

        // Wait a Frame
        yield return new WaitForFixedUpdate();

        // Create a new Map
        SetMap();

        // Print Matrix
        printMatrix();

        // Load all resources rooms in a Array
        resourcesEnemyRoomList.Clear();
        resourcesHallwayList.Clear();
        resourcesStartRoomList.Clear();

        resourcesEnemyRoomList = Resources.LoadAll<GameObject>("Rooms/Enemy Rooms").ToList();
        resourcesStartRoomList = Resources.LoadAll<GameObject>("Rooms/Start Rooms").ToList();
        resourcesHallwayList = Resources.LoadAll<GameObject>("Rooms/Hallway").ToList();

        // Spawn Rooms
        SpawnRooms();
    }

    public void SpawnRooms()
    {
        rooms = new List<GameObject>();

        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldLength; y++)
            {
                if (map[x, y] == 1) roomSpawner(new Vector2Int(x, y), findNeighbourRoomCount(new Vector2Int(x, y)));
            }
        }
    }

    public void roomSpawner(Vector2Int roomPos, int neighbourRoomCount)
    {
        GameObject r = new GameObject();
        List<GameObject> rList = new List<GameObject>();

        if (rooms.Count == 0)
        {
            switch (neighbourRoomCount)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    foreach (GameObject room in resourcesStartRoomList) if (room.GetComponent<EnemyRoom>().roomDirection == Room.RoomDirection.FourDoor) rList.Add(room);

                    r = Instantiate(resourcesStartRoomList[UnityEngine.Random.Range(0, resourcesStartRoomList.Count)], new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), Quaternion.identity, gameObject.transform) as GameObject;
                    break;

                default:
                    r = Instantiate(resourcesStartRoomList[UnityEngine.Random.Range(0, resourcesStartRoomList.Count)], new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), Quaternion.identity, gameObject.transform) as GameObject;
                    break;
            }

        }
        else
        {
            switch (neighbourRoomCount)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    foreach (GameObject room in resourcesEnemyRoomList) if (room.GetComponent<EnemyRoom>().roomDirection == Room.RoomDirection.FourDoor) rList.Add(room);

                    r = Instantiate(rList[UnityEngine.Random.Range(0, rList.Count)], new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), Quaternion.identity, gameObject.transform) as GameObject;
                    break;

                default:
                    r = Instantiate(resourcesEnemyRoomList[UnityEngine.Random.Range(0, resourcesEnemyRoomList.Count)], new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), Quaternion.identity, gameObject.transform) as GameObject;
                    break;

            }

        }

        rooms.Add(r);
    }

    public int findNeighbourRoomCount(Vector2Int roomPos)
    {
        int count = -1;

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (roomPos.x + x >= 0 && roomPos.x + x < worldWidth && roomPos.y + y >= 0 && roomPos.y + y < worldLength)
                    if (map[roomPos.x + x, roomPos.y + y] != 0) count += 1;
            }
        }

        return count;
    }

    public void SetMap()
    {
        mapSetPos.Clear();
        map = new int[worldWidth, worldLength];

        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldLength; y++)
            {
                map[x, y] = 0;
            }
        }


        int roomsLeft = roomCount;

        Vector2Int randomStartPos = new Vector2Int(UnityEngine.Random.Range(0, worldWidth), UnityEngine.Random.Range(0, worldLength));


        while (roomsLeft > 0)
        {
            if (roomsLeft == roomCount)
            {
                map[randomStartPos.x, randomStartPos.y] = 1;
                roomsLeft -= 1;
                mapSetPos.Add(randomStartPos);
            }
            else
            {
                while (setRandomRoom()) ;
                roomsLeft -= 1;
            }

        }
    }

    public bool setRandomRoom()
    {
        Vector2Int pos = getRandomRoomInMap();

        switch (RandomDir())
        {
            case Direction.Top:
                pos.y += 1;
                break;
            case Direction.Left:
                pos.x -= 1;
                break;
            case Direction.Right:
                pos.x += 1;
                break;
            case Direction.Down:
                pos.y -= 1;
                break;
        }

        if (!mapSetPos.Contains(pos) && pos.x >= 0 && pos.x < worldWidth && pos.y >= 0 && pos.y < worldLength)
        {
            mapSetPos.Add(pos);
            map[pos.x, pos.y] = 1;
            return false;
        }

        return true;
    }

    public Vector2Int getRandomRoomInMap()
    {
        return mapSetPos[UnityEngine.Random.Range(0, mapSetPos.Count - 1)];
    }

    public Direction RandomDir()
    {
        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                return Direction.Top;
            case 1:
                return Direction.Left;
            case 2:
                return Direction.Right;
            case 3:
                return Direction.Down;
        }

        return Direction.Top;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(generateMap());
    }

    private void printMatrix()
    {
        String matrix = "";

        for (int i = 0; i < worldWidth; i++)
        {
            String row = "";

            for (int j = 0; j < worldLength; j++)
            {
                row += map[i, j].ToString() + " ";
            }

            matrix += row + "\n";
        }

        UnityEngine.Debug.Log(matrix);
    }
}
