using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum callbackValue
    {
        SUCCESS,
        FAILURE,
        NOTHING
    }

    [Header("Weapon Default Stats")]
    public LayerMask DoDamageOn;
    public DamageType damageType;

    [SerializeField] public List<GameObject> hitObjects;
    [HideInInspector] public Animator animator;
    [HideInInspector] public string currentState;
    

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void callOnEquip(bool isSpawned);

    public abstract callbackValue callbackDamageFnc();

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

    public bool layermaskInclude(int layer)
    {
        return DoDamageOn == (DoDamageOn | (1 << layer));
    }
}
