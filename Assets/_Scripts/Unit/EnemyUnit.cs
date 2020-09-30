using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [HideInInspector] public UtilityAIHandler utilityAI;

    [HideInInspector] public bool isIdling;
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isWalking;

    public override void Start()
    {
        base.Start();
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
}
