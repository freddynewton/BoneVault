using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Stats/DefaultStat")]
public class Stats : ScriptableObject
{
    [Header("Stat Values")]
    public int health = 10;

    [Header("MovementSettings")]
    public float moveSpeed = 7f;

    [Header("Enemy Settings")]
    public float maxRange = 3f;
    public float stoppingDistance = 1.5f;

    [Header("Enemy Fighting Stats")]
    public bool stopsAttackingWhenHit = true;
    public int damage = 1;
    public float attackRate = 1.5f;

    [Header("PlayerStats")]
    public float stamina = 10;
    public float staminaRate = 1;

    [Header("Player movement")]
    public float mouseSensitivity = 500f;
    public float gravity = -9.81f;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 1f;
}
