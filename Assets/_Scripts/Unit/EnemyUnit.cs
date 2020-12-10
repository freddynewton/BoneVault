using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : Unit
{
    [Header("Enemy Settings")]
    public EnemyStats enemyStats;
    public DamageType damageType;
    public AudioClip [] hitSFX;
    public AudioClip [] attackSFX;
    public AudioClip [] dieSFX;

    [HideInInspector] public UtilityAIHandler utilityAI;
    [HideInInspector] public ParticleSystem vfx;
    [HideInInspector] public NavAgentController navAgent;
    [HideInInspector] public AudioSource audioSource;

    protected float waitTicker;
    protected float waitTime;


    public override void Start()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        baseMat = spriteRend.material;
        rb = GetComponent<Rigidbody>();

        navAgent = GetComponent<NavAgentController>();
        base.Start();
        utilityAI = GetComponent<UtilityAIHandler>();
        vfx = GetComponentInChildren<ParticleSystem>();
        vfx.Stop();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void Update()
    {
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

        SoundManager.Instance.playRandomSFX(dieSFX, audioSource, 0.8f, 1.2f);

        // Disable all component and leave a sprite
        GetComponent<UtilityAIHandler>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

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

        SoundManager.Instance.playRandomSFX(hitSFX, audioSource, 0.8f, 1.2f);
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
        StartCoroutine(flashWhite(0.1f));
        base.DoDamage(damageObj, damageType);
    }

    public virtual void setWalkingAnimation()
    {
        if (navAgent.agent.velocity != Vector3.zero) animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);
    }

    public bool waitTimer(float time)
    {
        waitTime = time;

        if (waitTicker > waitTime)
        {
            waitTicker = 0;
            return true;
        }
        else waitTicker += Time.deltaTime;

        return false;
    }

    public IEnumerator flashWhite(float time)
    {
        //spriteRend.material = Resources.Load("Material/White Shader Material") as Material;
        yield return new WaitForSeconds(time);
        StartCoroutine(freezeGame(0.035f));
        //spriteRend.material = baseMat;
    }
}