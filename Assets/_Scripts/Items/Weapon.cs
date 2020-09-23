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
    

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void attackLeftClick(bool active);

    public abstract void attackRightClick(bool active);

    // manages animation states
    public void changeAnimationState (string newState) {
        // prevent current animation interruption
        if (currentState == newState) return;

        // play the invoked animation
        animator.Play(newState);

        // set string to current animation as a monitor
        currentState = newState;
    }
}
