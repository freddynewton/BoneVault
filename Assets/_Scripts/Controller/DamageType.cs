using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Damage Type")]
public class DamageType : ScriptableObject
{
    [Header("Damage Settings")]
    public int damage;

    [Header("Bools")]
    public bool isBlockable;
    public bool isShootingBackWhenHit;

    [Header("Knockback")]
    public float knockbackForce;

}
