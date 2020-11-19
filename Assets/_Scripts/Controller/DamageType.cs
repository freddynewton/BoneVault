using UnityEngine;

[CreateAssetMenu(menuName = "Damage Type")]
public class DamageType : ScriptableObject
{
    [Header("Damage Settings")]
    public int damage;

    [Header("Costs")]
    public float staminaCost = 0;

    [Header("Bools")]
    public bool isBlockable;

    public bool isShootingBackWhenHit;

    [Header("Knockback")]
    public float knockbackForce;
}