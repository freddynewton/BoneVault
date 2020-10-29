using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapPart : MonoBehaviour
{

    public enum RoomDirMM
    {
        OneDoor,
        TwoDoorLinear,
        TwoDoorCurve,
        ThreeDoor,
        FourDoor,
        Hallway
    }

    public Room room;
    public Image icon;

    public void setPart(Room _room)
    {
        room = _room;
        room.miniMapPart = this;
        StartCoroutine(setIcon());
    }

    private IEnumerator setIcon()
    {
        yield return new WaitForEndOfFrame();
        if (room != null)
        {
            if (room.roomType != Room.RoomType.ENEMIE_ROOM && room.roomType != Room.RoomType.HALLWAY)
            {
                icon.sprite = getIcon(room.roomType);
                icon.preserveAspect = true;
            }

        }

        gameObject.SetActive(false);
    }

    // Get Sprites todo
    public Sprite getIcon(Room.RoomType type)
    {
        if (icon != null)
        {
            icon.enabled = true;

            switch (type)
            {
                case Room.RoomType.START_ROOM:
                    return Resources.Load<Sprite>("UI/Health");
                case Room.RoomType.ENEMIE_ROOM:
                    break;
                case Room.RoomType.BOSS_ROOM:
                    return Resources.Load<Sprite>("UI/Passive");
                case Room.RoomType.SPECIAL_ROOM:
                    return Resources.Load<Sprite>("UI/Stamina");
            }
        }

        return null;
    }



    [Header("Part Settigns")]
    public RoomDirMM roomDirection;
}
