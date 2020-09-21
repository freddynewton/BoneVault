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
    [HideInInspector] public string currentState;
    [HideInInspector] public float animationLength;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isBlocking;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void attackLeftClick()
    {
        if (!isBlocking) changeAnimationState("Attack1");
        animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("attackComplete", animationLength);
    }

    public virtual void attackRightClick()
    {
        if (!isAttacking) changeAnimationState("Block1");
        animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public virtual void idle() {
        if (!isAttacking && !isBlocking) changeAnimationState("Idle");
    }

    public virtual void move() {
        if (!isAttacking && !isBlocking) changeAnimationState("Walk");
    }

    // manages animation states
    public virtual void changeAnimationState (string newState) {
        // prevent current animation interruption
        if (currentState == newState) return;

        // play the invoked animation
        animator.Play(newState);

        // set string to current animation as a monitor
        currentState = newState;
    }

    public virtual void attackComplete() {
        isAttacking = false;
    }

    public virtual void blockComplete () {
        isBlocking = false;
    }
}
