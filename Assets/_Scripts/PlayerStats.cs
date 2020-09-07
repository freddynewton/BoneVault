using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Stats/PlayerStat")]
public class PlayerStats : Stats
{
    [Header("PlayerStats")]
    public float mouseSensitivity = 100f;
    public float gravity = -9.81f;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 2f;
}
