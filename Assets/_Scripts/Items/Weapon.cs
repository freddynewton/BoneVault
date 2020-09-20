using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Default Stats")]
    public LayerMask DoDamageOn;
    public int Damage;

    [HideInInspector] public Animator animator;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void attackLeftClick();

    public abstract void attackRightClick();
}
