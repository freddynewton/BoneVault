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
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isBlocking;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void attackLeftClick();

    public virtual void attackRightClick()
    {
        if (!isAttacking) {
            int randomInt = Random.Range(1, 3);
            if (randomInt == 1) changeAnimationState("Block1");
            else changeAnimationState("Block2");
        }
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
        Debug.Log(Inventory.Instance.currWeaponScript.isAttacking);
    }

    public virtual void blockComplete() {
        isBlocking = false;
    }
}
