using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Stats/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("PlayerStats")]
    public float stamina = 10;

    public float staminaRate = 1;
    public float sprintCostRate = 2f;

    [Header("Player movement")]
    public float mouseSensitivity = 500f;

    public float gravity = -9.81f;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 1f;
}
