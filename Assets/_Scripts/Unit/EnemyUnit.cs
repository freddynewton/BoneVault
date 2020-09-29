using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public UtilityAIHandler utilityAI;
    [HideInInspector] public string currentState;

    [HideInInspector] public bool isIdling;
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isWalking;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        utilityAI = GetComponent<UtilityAIHandler>();
    }

    public override void Update()
    {
        base.Update();
        setWalkingAnimation();
    }

    public override void death()
    {
        isDead = true;

        // Play death anim
        // here

        base.death();
    }

    private void setWalkingAnimation()
    {
        if (!isHit && !isDead && !isAttacking)
        {
            if (utilityAI.navAgent.velocity != Vector3.zero)
            {
                isWalking = true;
                isIdling = false;
                changeAnimationState("Walk");
            }
            else
            {
                isWalking = false;
                isIdling = true;
                changeAnimationState("Idle");
            }
        } else
        {
            isWalking = false;
            isIdling = false;
        }
    }

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
