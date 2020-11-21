using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : Unit
{
    [Header("Animation Duration")]
    public float hitAnimationDuration;

    public float isAttackingDuration;

    [HideInInspector] public UtilityAIHandler utilityAI;
    private ParticleSystem vfx;

    [Header("Damage")]
    public DamageType damageType;

    public List<GameObject> triggerList = new List<GameObject>();

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

    public void CallTriggerDamage()
    {
        foreach (GameObject obj in triggerList)
        {
            obj.GetComponent<Unit>().DoDamage(gameObject, damageType);
        }
    }

    public virtual void setWalkingAnimation()
    {
        if (utilityAI.navAgent.velocity != Vector3.zero) animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggerList.Contains(other.gameObject)) triggerList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && triggerList.Contains(other.gameObject)) triggerList.Remove(other.gameObject);
    }
}