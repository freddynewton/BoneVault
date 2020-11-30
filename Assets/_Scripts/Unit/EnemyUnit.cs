using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : Unit
{
    [Header("Damage")]
    public DamageType damageType;
    public AudioClip [] hitSFX;
    public AudioClip [] attackSFX;

    [HideInInspector] public UtilityAIHandler utilityAI;
    [HideInInspector] public ParticleSystem vfx;
    [HideInInspector] public NavAgentController navAgent;
    [HideInInspector] public AudioSource randomSound;

    public override void Start()
    {
        navAgent = GetComponent<NavAgentController>();
        base.Start();
        utilityAI = GetComponent<UtilityAIHandler>();
        vfx = GetComponentInChildren<ParticleSystem>();
        vfx.Stop();
        randomSound = GetComponentInChildren<AudioSource>();
    }

    public override void Update()
    {
        base.Update();
        FaceTarget(PlayerController.Instance.transform.position);
        setWalkingAnimation();
    }

    public override void death()
    {
        // Play Death Animation
        animator.SetTrigger("isDead");

        // Play Hit Particle
        if (vfx != null)
        {
            vfx.Clear();
            vfx.Play();
        }

        playRandomSFX(hitSFX);

        // Disable all component and leave a sprite
        GetComponent<UtilityAIHandler>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

        base.death();

        Inventory.Instance.setBones(Random.Range(0, 2));
    }

    public override void hit()
    {
        // Play Hit Animation
        animator.SetTrigger("Hit");

        // Play Hit Particle
        if (vfx != null)
        {
            vfx.Clear();
            vfx.Play();
        }

        playRandomSFX(hitSFX);

        base.hit();
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }

    public override void DoDamage(GameObject damageObj, DamageType damageType)
    {
        base.DoDamage(damageObj, damageType);
    }

    public override void knockback(Vector3 otherPos, float kb)
    {
        base.knockback(otherPos, kb);
    }

    public virtual void setWalkingAnimation()
    {
        if (navAgent.agent.velocity != Vector3.zero) animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);
    }

    // SFX Handler
    private void playRandomSFX (AudioClip [] sounds) {
        randomSound.clip = sounds [Random.Range(0, sounds.Length)];
        randomSound.pitch = Random.Range(0.8f, 1.2f);

        if (randomSound != null) randomSound.Play();
    }
}