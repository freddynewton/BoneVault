using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Stats/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Enemy Settings")]
    public float maxRange = 3f;
    public float stoppingDistance = 1.5f;

    [Header("Enemy Fighting Stats")]
    public bool stopsAttackingWhenHit = true;

    public float attackRateMin = 1.5f;
    public float attackRateMax = 3f;
}
