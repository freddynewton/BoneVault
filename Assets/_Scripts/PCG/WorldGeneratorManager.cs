using System.Collections;
using System.Collections.Generic;
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

    [Header("World Size Settings")]
    public int roomCount;
    public int worldWidth = 10;
    public int worldLength = 10;

    [HideInInspector] public int[,] map;
    [HideInInspector] public List<Vector2Int> mapSetPos;

    public IEnumerator generateMap()
    {
        // Delete old map
        foreach (Transform child in gameObject.transform) Destroy(child.gameObject);

        // Wait a Frame
        yield return new WaitForFixedUpdate();

        // Create a new Map
        SetMap();

    }

    public void SetMap()
    {
        mapSetPos.Clear();
        map = new int[worldWidth, worldLength];

        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldLength; y++)
            {
                map[worldWidth, worldLength] = 0;
            }
        }

        int roomsLeft = roomCount;

        Vector2Int randomStartPos = new Vector2Int(Random.Range(0, worldWidth), Random.Range(0, worldLength));


        while (roomsLeft > 0)
        {
            if (roomsLeft == roomCount)
            {
                map[randomStartPos.x, randomStartPos.y] = 1;
                roomsLeft -= 1;
                mapSetPos.Add(randomStartPos);
                continue;
            }

            while (setRandomRoom()) ;
            roomsLeft -= 1;
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

        if (mapSetPos.Contains(pos)) { mapSetPos.Add(pos); return true; }

        return false;
    }

    public Vector2Int getRandomRoomInMap()
    {
        return mapSetPos[Random.Range(0, mapSetPos.Count - 1)];
    }

    public Direction RandomDir()
    {
        switch (Random.Range(0, 4))
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
    }

}
