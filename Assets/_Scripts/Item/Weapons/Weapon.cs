using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    public enum callbackValue
    {
        SUCCESS,
        FAILURE,
        NOTHING
    }

    [Header("Weapon Default Stats")]
    public DamageType damageType;

    [Header("Screenshake Settings")]
    public float ShakeLength = 1;
    public float ShakeStrength = 2;

    [HideInInspector] public Animator animator;
    [HideInInspector] public string currentState;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    /* TODO: Implement Interact for item grabbing*/
    public override void interact()
    {
        throw new System.NotImplementedException();
    }

    public abstract void callOnEquip(bool isSpawned);

    public abstract callbackValue callbackDamageFnc();

    public abstract void ability1(bool active);

    public abstract void ability2(bool active);

    // manages animation states
    public void changeAnimationState(string newState)
    {
        // prevent current animation interruption
        if (currentState == newState) return;

        // play the invoked animation
        animator.Play(newState);

        // set string to current animation as a monitor
        currentState = newState;
    }
}