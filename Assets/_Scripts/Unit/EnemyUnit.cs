using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : Unit
{
    [Header("Animation Duration")]
    public float hitAnimationDuration;
    public float isAttackingDuration;

    [HideInInspector] public UtilityAIHandler utilityAI;
    [HideInInspector] private ParticleSystem vfx;

    [HideInInspector] public bool isIdling;
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isWalking;

    public override void Start()
    {
        base.Start();
        utilityAI = GetComponent<UtilityAIHandler>();
        vfx = GetComponentInChildren<ParticleSystem>();
        vfx.Stop();
    }

    public override void Update()
    {
        base.Update();
        setWalkingAnimation();
    }

    public override void death()
    {
        isDead = true;
        changeAnimationState("Death");
        GetComponent<UtilityAIHandler>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        base.death();
    }

    public override void hit()
    {
        changeAnimationState("Hit");
        vfx.Clear();
        vfx.Play();
        base.hit();
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
