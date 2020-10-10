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
    
    public void setDoors(bool isOpen)
    {
        foreach(Door d in doors)
        {
            if (isOpen) d.openDoor();
            else d.closeDoor();
        }
    }
}
