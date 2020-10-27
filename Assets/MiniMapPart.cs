using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Part Settigns")]
    public RoomDirMM roomDirection;
}
