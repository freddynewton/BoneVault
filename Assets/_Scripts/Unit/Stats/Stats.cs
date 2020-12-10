using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Stats/Base Stats")]
public class Stats : ScriptableObject
{
    [Header("Stat Values")]
    public int maxHealth = 10;

    [Header("MovementSettings")]
    public float moveSpeed = 7f;
}