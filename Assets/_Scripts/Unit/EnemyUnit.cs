using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public UtilityAIHandler utilityAI;
    [HideInInspector] public string currentState;

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

    private void setWalkingAnimation()
    {
        if (utilityAI.navAgent.velocity != Vector3.zero) {
            changeAnimationState("Walk");
        }
        else changeAnimationState("Idle");
    }

    public void changeAnimationState (string newState) {
        // prevent current animation interruption
        if (currentState == newState) return;

        // play the invoked animation
        animator.Play(newState);

        // set string to current animation as a monitor
        currentState = newState;
    }
}
