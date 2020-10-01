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

    public List<GameObject> playerList = new List<GameObject>();

    // Bodyless functions
    public void changeHitBool() => isHit = !isHit;
    public void changeAttackingBool() => isAttacking = !isAttacking;

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

        // Play Death Animation
        changeAnimationState("Death");

        // Play Hit Particle
        vfx.Clear();
        vfx.Play();

        // Disable all component and leave a sprite
        GetComponent<UtilityAIHandler>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

        base.death();
    }

    public override void hit()
    {
        isHit = true;

        // Play Hit Animation
        changeAnimationState("Hit");

        // Imitate Animation Event
        Invoke("changeHitBool", hitAnimationDuration);
        
        // Play Hit Particle
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerList.Contains(other.gameObject)) playerList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerList.Contains(other.gameObject)) playerList.Remove(other.gameObject);
    }
}
