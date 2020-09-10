using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public UtilityAIHandler utilityAI;

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
        if (utilityAI.navAgent.velocity != Vector3.zero) animator.SetBool("walking", true);
        else animator.SetBool("walking", false);
    }
}
