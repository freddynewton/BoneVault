using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Stats/DefaultStat")]
public class Stats : ScriptableObject
{
    [Header("Stat Values")]
    public int Health = 10;

    [Header("MovementSettings")]
    public float movementSpeed = 5f;
}
