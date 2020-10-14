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

    /// <summary>
    /// Generate Map
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Get Next room Spawn
    /// </summary>
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

    /// <summary>
    /// Spawns all Rooms
    /// </summary>
    /// <param name="roomPos"></param>
    /// <param name="neighbourRoomCount"></param>
    public void roomSpawner(Vector2Int roomPos, int neighbourRoomCount)
    {
        GameObject r = new GameObject();

        if (rooms.Count == 0)
        {
            r = Instantiate(returnRandomRoom(neighbourRoomCount, 0, roomPos), new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), returnRoomRotation(neighbourRoomCount, roomPos), gameObject.transform) as GameObject;
        }
        else
        {
            r = Instantiate(returnRandomRoom(neighbourRoomCount, 1, roomPos), new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), returnRoomRotation(neighbourRoomCount, roomPos), gameObject.transform) as GameObject;
        }

        rooms.Add(r);
    }

    /// <summary>
    /// returns a Random room
    /// </summary>
    /// <param name="neighbourRoomCount">Room Count</param>
    /// <param name="roomType">
    /// 
    /// 0 = Start Room
    /// 1 = Enemy Room
    /// 
    /// </param>
    /// <returns></returns>
    public GameObject returnRandomRoom(int neighbourRoomCount, int roomType, Vector2Int roomPos)
    {
        List<GameObject> rList = new List<GameObject>();
        List<GameObject> roomList = new List<GameObject>();

        switch (roomType)
        {
            case 0:
                roomList = resourcesStartRoomList;
                break;
            case 1:
                roomList = resourcesEnemyRoomList;
                break;
        }

        switch (neighbourRoomCount)
        {
            case 1:
                foreach (GameObject room in roomList)
                    if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.OneDoor) rList.Add(room);
                break;

            case 2:
                foreach (GameObject room in roomList)
                {
                    if (roomPos.y + 1 < worldLength && roomPos.y - 1 >= 0)
                        if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x, roomPos.y - 1] == 1)) if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.TwoDoorLinear) rList.Add(room);

                    if (roomPos.x + 1 < worldWidth && roomPos.x - 1 >= 0)
                        if (map[roomPos.x + 1, roomPos.y] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.TwoDoorLinear) rList.Add(room);

                    if (roomPos.y + 1 < worldLength && roomPos.x + 1 < worldWidth)
                        if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x + 1, roomPos.y] == 1)) if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.TwoDoorCurve) rList.Add(room);

                    if (roomPos.x + 1 < worldWidth && roomPos.y - 1 >= 0)
                        if (map[roomPos.x, roomPos.y - 1] == 1 && (map[roomPos.x + 1, roomPos.y] == 1)) if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.TwoDoorCurve) rList.Add(room);

                    if (roomPos.x - 1 >= 0 && roomPos.y - 1 >= 0)
                        if (map[roomPos.x, roomPos.y - 1] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.TwoDoorCurve) rList.Add(room);

                    if (roomPos.y + 1 < worldLength && roomPos.x - 1 >= 0)
                        if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.TwoDoorCurve) rList.Add(room);
                }
                break;

            case 3:
                foreach (GameObject room in roomList)
                    if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.ThreeDoor) rList.Add(room);
                break;

            case 4:
                foreach (GameObject room in roomList)
                    if (room.GetComponent<Room>().roomDirection == Room.RoomDirection.FourDoor) rList.Add(room);
                break;
        }

        return rList[UnityEngine.Random.Range(0, rList.Count - 1)];
    }

    /// <summary>
    /// Return a Rotation to set the rooms positon
    /// </summary>
    /// <param name="neighbourRoomCount"></param>
    /// <param name="roomPos"></param>
    /// <returns></returns>
    public Quaternion returnRoomRotation(int neighbourRoomCount, Vector2Int roomPos)
    {
        Quaternion q = new Quaternion();

        switch (neighbourRoomCount)
        {
            case 1:

                if (roomPos.x + 1 < worldWidth)
                    if (map[roomPos.x + 1, roomPos.y] == 1) q = Quaternion.Euler(0, 90, 0);

                if (roomPos.x - 1 >= 0)
                    if (map[roomPos.x - 1, roomPos.y] == 1) q = Quaternion.Euler(0, 270, 0);

                if (roomPos.y - 1 >= 0)
                    if (map[roomPos.x, roomPos.y - 1] == 1) q = Quaternion.Euler(0, 180, 0);

                if (roomPos.y + 1 < worldLength)
                    if (map[roomPos.x, roomPos.y + 1] == 1) q = Quaternion.identity;
                break;

            case 2:
                if (roomPos.y + 1 < worldLength && roomPos.y - 1 >= 0)
                    if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x, roomPos.y - 1] == 1)) q = Quaternion.identity;

                if (roomPos.x + 1 < worldWidth && roomPos.x - 1 >= 0)
                    if (map[roomPos.x + 1, roomPos.y] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) q = Quaternion.Euler(0, 90, 0);

                if (roomPos.y + 1 < worldLength && roomPos.x + 1 < worldWidth)
                    if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x + 1, roomPos.y] == 1)) q = Quaternion.identity;

                if (roomPos.x + 1 < worldWidth && roomPos.y - 1 >= 0)
                    if (map[roomPos.x, roomPos.y - 1] == 1 && (map[roomPos.x + 1, roomPos.y] == 1)) q = Quaternion.Euler(0, 90, 0);

                if (roomPos.x - 1 >= 0 && roomPos.y - 1 >= 0)
                    if (map[roomPos.x, roomPos.y - 1] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) q = Quaternion.Euler(0, 180, 0);

                if (roomPos.y + 1 < worldLength && roomPos.x - 1 >= 0)
                    if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) q = Quaternion.Euler(0, 270, 0);
                break;

            case 3:
                if (roomPos.x + 1 < worldWidth)
                {
                    if (map[roomPos.x + 1, roomPos.y] == 0) q = Quaternion.Euler(0, 180, 0);
                }
                else q = Quaternion.Euler(0, 180, 0);

                if (roomPos.x - 1 >= 0)
                {
                    if (map[roomPos.x - 1, roomPos.y] == 0) q = Quaternion.identity;
                }
                else q = Quaternion.identity;

                if (roomPos.y - 1 >= 0)
                {
                    if (map[roomPos.x, roomPos.y - 1] == 0) q = Quaternion.Euler(0, 270, 0);
                }
                else q = Quaternion.Euler(0, 270, 0);

                if (roomPos.y + 1 < worldLength)
                {
                    if (map[roomPos.x, roomPos.y + 1] == 0) q = Quaternion.Euler(0, 90, 0);
                }
                else q = Quaternion.Euler(0, 90, 0);

                break;

            case 4:
                int random = UnityEngine.Random.Range(0, 5);

                q = Quaternion.Euler(0, 90 * random, 0);
                break;
        }

        return q;
    }

    /// <summary>
    /// Return Neighbour Room Count
    /// </summary>
    /// <param name="roomPos"></param>
    /// <returns></returns>
    public int findNeighbourRoomCount(Vector2Int roomPos)
    {
        int count = 0;

        if (roomPos.x + 1 < worldWidth)
            if (map[roomPos.x + 1, roomPos.y] == 1 && roomPos.x >= 0) count += 1;

        if (roomPos.y - 1 >= 0)
            if (map[roomPos.x, roomPos.y - 1] == 1) count += 1;

        if (roomPos.x - 1 >= 0)
            if (map[roomPos.x - 1, roomPos.y] == 1) count += 1;

        if (roomPos.y + 1 < worldLength)
            if (map[roomPos.x, roomPos.y + 1] == 1) count += 1;

        return count;
    }

    /// <summary>
    /// Generate 2D Int Array with binary structure
    /// </summary>
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

    /// <summary>
    /// Random room walker
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Returns random set room
    /// </summary>
    /// <returns></returns>
    public Vector2Int getRandomRoomInMap()
    {
        return mapSetPos[UnityEngine.Random.Range(0, mapSetPos.Count - 1)];
    }

    /// <summary>
    /// Random Direction
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Awake
    /// </summary>
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

    /// <summary>
    /// Prints Map for Debug
    /// </summary>
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
