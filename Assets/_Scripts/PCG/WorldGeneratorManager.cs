using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.PlayerLoop;

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
    public int specialRoomCount = 1;
    public int worldWidth = 10;
    public int worldLength = 10;

    [Header("Placement Settings")]
    public const int roomSpace = 96;
    public const int hallwaySpace = 48;

    [Header("Developer Settings")]
    public bool TestMode = false;

    [HideInInspector] public int[,] map;
    [HideInInspector] public List<Vector2Int> mapSetPos;

    private List<GameObject> rooms = new List<GameObject>();
    private List<GameObject> hallWays = new List<GameObject>();

    private List<GameObject> resourcesEnemyRoomList = new List<GameObject>();
    private List<GameObject> resourcesStartRoomList = new List<GameObject>();
    private List<GameObject> resourcesHallwayList = new List<GameObject>();
    private List<GameObject> resourcesSpecialRooms = new List<GameObject>();

    private List<NavMeshSurface> navMeshSurfaces = new List<NavMeshSurface>();

    private void LateUpdate()
    {
        if (TestMode && Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(generateMap());
    }

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
        resourcesSpecialRooms.Clear();

        resourcesEnemyRoomList = Resources.LoadAll<GameObject>("Rooms/Enemy Rooms").ToList();
        resourcesStartRoomList = Resources.LoadAll<GameObject>("Rooms/Start Rooms").ToList();
        resourcesHallwayList = Resources.LoadAll<GameObject>("Rooms/Hallways").ToList();
        resourcesSpecialRooms = Resources.LoadAll<GameObject>("Rooms/Special Rooms").ToList();

        // Spawn Rooms
        SpawnRooms();

        // Spawn Special Rooms
        SpawnSpecialRooms();

        // Set Hallway
        SpawnHallways();

        // Set Playerpos
        foreach (GameObject r in rooms)
        {
            StartRoom sr = r.GetComponent<StartRoom>();

            if (sr != null)
            {
                PlayerController.Instance.transform.position = sr.PlayerSpawn.position;
            }
        }

        // Bake Navmesh
        bakeNavmesh();
    }

    /// <summary>
    /// Bake navmesh while runtime
    /// </summary>
    public void bakeNavmesh()
    {
        navMeshSurfaces.Clear();

        navMeshSurfaces = gameObject.GetComponents<NavMeshSurface>().ToList();

        foreach (NavMeshSurface n in navMeshSurfaces) n.BuildNavMesh();
    }

    /// <summary>
    /// Spawning Hallways
    /// </summary>
    public void SpawnHallways()
    {
        hallWays.Clear();

        foreach (GameObject r in rooms)
        {
            EnemyRoom room = r.GetComponent<EnemyRoom>();

            if (room != null)
            {
                foreach (Door d in room.doors)
                {
                    Vector3 dir = -(r.gameObject.transform.position - d.gameObject.transform.position).normalized;

                    if (controlHallwayPos(r.gameObject.transform.position + dir * hallwaySpace))
                        hallWays.Add(Instantiate(returnRandomHallway(), r.gameObject.transform.position + dir * hallwaySpace, returnHallwayRotation(dir), gameObject.transform) as GameObject); ;
                }
            }
        }
    }

    /// <summary>
    /// Return rotation for Hallways
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Quaternion returnHallwayRotation(Vector3 dir)
    {
        Quaternion q = Quaternion.identity;

        Debug.Log(dir);

        if (dir.z == 1 || dir.z == -1)
        {
            q = Quaternion.Euler(0, 90, 0);
        }

        return q;
    }


    /// <summary>
    /// Return Random Hallway Object
    /// </summary>
    /// <returns></returns>
    private GameObject returnRandomHallway()
    {
        return resourcesHallwayList[UnityEngine.Random.Range(0, resourcesHallwayList.Count)];
    }

    /// <summary>
    /// Check if on the Pos a Hallway already spawned
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool controlHallwayPos(Vector3 pos)
    {
        bool found = true;

        foreach (GameObject h in hallWays)
        {
            if (pos == h.gameObject.transform.position) found = false;
        }

        return found;
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

    // Spawn all special rooms
    public void SpawnSpecialRooms()
    {
        for (int i = 0; i < specialRoomCount; i++)
        {
            bool t = false;

            while (!t)
            {
                Vector2Int tmp = getRandomRoomInMap();
                Vector3 roomPos = new Vector3(tmp.x * roomSpace, 0, tmp.y * roomSpace);

                if ((findNeighbourRoomCount(tmp) == 1 || findNeighbourRoomCount(tmp) == 3) && rooms[0].gameObject.transform.position != roomPos)
                {
                    t = setSpecialRoom(tmp, roomPos);
                }
            }
        }
    }

    /// <summary>
    /// Get Room in World Pos
    /// </summary>
    /// <param name="roomPos"></param>
    /// <returns></returns>
    public GameObject getRoomOnPos(Vector3 roomPos)
    {
        foreach (GameObject r in rooms)
        {
            if (r.transform.position == roomPos) return r;
        }

        return new GameObject();
    }

    /// <summary>
    /// Returns a direction where the is not a Room
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2Int getZeroInMap(Vector2Int pos, int count)
    {
        Vector2Int ret = new Vector2Int();

        if (count == 3)
        {
            try
            {
                if (map[pos.x, pos.y + 1] == 0)
                {
                    return new Vector2Int(pos.x, pos.y + 1);
                }
            }
            catch (Exception)
            {
                return new Vector2Int(pos.x, pos.y + 1);
            }

            try
            {

                if (map[pos.x, pos.y - 1] == 0)
                {
                    return new Vector2Int(pos.x, pos.y - 1);
                }
            }
            catch (Exception)
            {
                return new Vector2Int(pos.x, pos.y - 1);
            }


            try
            {
                if (map[pos.x + 1, pos.y] == 0)
                {
                    return new Vector2Int(pos.x + 1, pos.y);
                }
            }
            catch (Exception)
            {
                return new Vector2Int(pos.x + 1, pos.y);
            }

            try
            {
                if (map[pos.x - 1, pos.y] == 0)
                {
                    return new Vector2Int(pos.x - 1, pos.y);
                }
            }
            catch (Exception)
            {
                return new Vector2Int(pos.x - 1, pos.y);
            }
        }
        else if (count == 1)
        {
            try
            {
                if (map[pos.x, pos.y + 1] == 1)
                {
                    return new Vector2Int(pos.x, pos.y - 1);
                }
            }
            catch (Exception)
            {
                
            }

            try
            {

                if (map[pos.x, pos.y - 1] == 1)
                {
                    return new Vector2Int(pos.x, pos.y + 1);
                }
            }
            catch (Exception)
            {
                
            }


            try
            {
                if (map[pos.x + 1, pos.y] == 1)
                {
                    return new Vector2Int(pos.x - 1, pos.y);
                }
            }
            catch (Exception)
            {
                
            }

            try
            {
                if (map[pos.x - 1, pos.y] == 1)
                {
                    return new Vector2Int(pos.x + 1, pos.y);
                }
            }
            catch (Exception)
            {
                
            }
        }
        return pos;
    }

    /// <summary>
    /// Replace Enemy Room and place a new one
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="roomPos"></param>
    public bool setSpecialRoom(Vector2Int pos, Vector3 roomPos)
    {
        List<GameObject> roomL = new List<GameObject>();

        GameObject room = getRoomOnPos(roomPos);

        int neighbourPos = findNeighbourRoomCount(pos);

        if (neighbourPos == 1)
        {
            foreach (GameObject f in resourcesEnemyRoomList)
            {
                if (f.GetComponent<Room>().roomDirection == Room.RoomDirection.TwoDoorLinear) roomL.Add(f);
            }
        }
        else if (neighbourPos == 3)
        {
            foreach (GameObject f in resourcesEnemyRoomList)
            {
                if (f.GetComponent<Room>().roomDirection == Room.RoomDirection.FourDoor) roomL.Add(f);
            }

        }

        Vector2Int specialRoomPos = new Vector2Int();

        switch (neighbourPos)
        {
            case 1:
                specialRoomPos = getZeroInMap(pos, 1);
                GameObject _spc = Instantiate(resourcesSpecialRooms[UnityEngine.Random.Range(0, resourcesSpecialRooms.Count)], new Vector3(specialRoomPos.x * roomSpace, 0, specialRoomPos.y * roomSpace), Quaternion.identity, gameObject.transform) as GameObject;
                GameObject _new = Instantiate(roomL[UnityEngine.Random.Range(0, roomL.Count)], roomPos, room.transform.rotation, gameObject.transform) as GameObject;
                _spc.transform.LookAt(_new.transform);

                rooms.Add(_new);
                rooms.Add(_spc);


                break;
            case 3:
                specialRoomPos = getZeroInMap(pos, 3);

                GameObject specialRoom = Instantiate(resourcesSpecialRooms[UnityEngine.Random.Range(0, resourcesSpecialRooms.Count)], new Vector3(specialRoomPos.x * roomSpace, 0, specialRoomPos.y * roomSpace), Quaternion.identity, gameObject.transform) as GameObject;
                GameObject newRoom = Instantiate(roomL[UnityEngine.Random.Range(0, roomL.Count)], roomPos, Quaternion.identity, gameObject.transform) as GameObject;
                specialRoom.transform.LookAt(newRoom.transform);

                rooms.Add(specialRoom);
                rooms.Add(newRoom);

                break;

        }

        rooms.Remove(room);
        Destroy(room);

        return true;
    }

    /// <summary>
    /// Spawns all Rooms
    /// </summary>
    /// <param name="roomPos"></param>
    /// <param name="neighbourRoomCount"></param>
    public void roomSpawner(Vector2Int roomPos, int neighbourRoomCount)
    {
        if (rooms.Count == 0)
        {
            rooms.Add(Instantiate(returnRandomRoom(neighbourRoomCount, 0, roomPos), new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), returnRoomRotation(neighbourRoomCount, roomPos), gameObject.transform) as GameObject);
        }
        else
        {
            rooms.Add(Instantiate(returnRandomRoom(neighbourRoomCount, 1, roomPos), new Vector3(roomPos.x * roomSpace, 0, roomPos.y * roomSpace), returnRoomRotation(neighbourRoomCount, roomPos), gameObject.transform) as GameObject);
        }
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

        foreach (GameObject room in roomList)
        {
            Room.RoomDirection roomDir = room.GetComponent<Room>().roomDirection;


            switch (neighbourRoomCount)
            {
                case 1:
                    if (roomDir == Room.RoomDirection.OneDoor) rList.Add(room);
                    break;

                case 2:

                    if (roomDir == Room.RoomDirection.TwoDoorLinear)
                    {
                        if (roomPos.y + 1 < worldLength && roomPos.y - 1 >= 0)
                        {
                            if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x, roomPos.y - 1] == 1)) rList.Add(room);
                        }

                        if (roomPos.x + 1 < worldWidth && roomPos.x - 1 >= 0)
                        {
                            if (map[roomPos.x + 1, roomPos.y] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) rList.Add(room);
                        }
                    }

                    if (roomDir == Room.RoomDirection.TwoDoorCurve)
                    {
                        if (roomPos.y + 1 < worldLength && roomPos.x + 1 < worldWidth)
                        {
                            if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x + 1, roomPos.y] == 1)) rList.Add(room);
                        }

                        if (roomPos.x + 1 < worldWidth && roomPos.y - 1 >= 0)
                        {
                            if (map[roomPos.x, roomPos.y - 1] == 1 && (map[roomPos.x + 1, roomPos.y] == 1)) rList.Add(room);
                        }

                        if (roomPos.x - 1 >= 0 && roomPos.y - 1 >= 0)
                        {
                            if (map[roomPos.x, roomPos.y - 1] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) rList.Add(room);
                        }

                        if (roomPos.y + 1 < worldLength && roomPos.x - 1 >= 0)
                        {
                            if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) rList.Add(room);
                        }
                    }
                    break;

                case 3:

                    if (roomDir == Room.RoomDirection.ThreeDoor) rList.Add(room);
                    break;

                case 4:

                    if (roomDir == Room.RoomDirection.FourDoor) rList.Add(room);
                    break;
            }
        }

        if (neighbourRoomCount == 2)
        {
            /*
            if (roomPos.y + 1 < worldLength && roomPos.y - 1 >= 0)
                if (map[roomPos.x, roomPos.y + 1] == 1 && (map[roomPos.x, roomPos.y - 1] == 1)) q = Quaternion.identity;

            if (roomPos.x + 1 < worldWidth && roomPos.x - 1 >= 0)
                if (map[roomPos.x + 1, roomPos.y] == 1 && (map[roomPos.x - 1, roomPos.y] == 1)) q = Quaternion.Euler(0, 90, 0);
            */
        }

        if (rList.Count == 0)
            Debug.Log("rList Count " + rList.Count + "Door Count: " + neighbourRoomCount);

        return rList[UnityEngine.Random.Range(0, rList.Count - 1 == 0 ? 1 : rList.Count - 1)];
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
