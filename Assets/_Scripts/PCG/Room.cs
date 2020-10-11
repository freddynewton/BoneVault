using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        START_ROOM,
        ENEMIE_ROOM,
        BOSS_ROOM,
        EXIT_ROOM
    }

    [Header("Basic Room Settings")]
    public RoomType roomType;
    public List<Door> doors;

    [Header("Room Light Settings")]
    public List<Light> lights;
    public float activateLightDelay = 0.2f;
    public Color mainColor;
    public Color secColor;

    
    
    public void setDoors(bool isOpen)
    {
        foreach(Door d in doors)
        {
            if (isOpen) d.openDoor();
            else d.closeDoor();
        }
    }
}
