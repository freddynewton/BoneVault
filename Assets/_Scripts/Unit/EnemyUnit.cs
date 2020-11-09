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
        setWalkingAnimation();
    }

    public override void death()
    {

        // Play Death Animation
        animator.SetTrigger("isDead");

        // Play Hit Particle
        vfx.Clear();
        vfx.Play();

        // Disable all component and leave a sprite
        GetComponent<UtilityAIHandler>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

        base.death();

        Inventory.Instance.setBones(1);
    }

    public override void hit()
    {
        // Play Hit Animation
        animator.SetTrigger("Hit");

        // Play Hit Particle
        vfx.Clear();
        vfx.Play();

        base.hit();
    }

    public override void DoDamage(Vector3 damageSrcPos, DamageType damageType)
    {
        base.DoDamage(damageSrcPos, damageType);
    }

    public override void knockback(Vector3 otherPos, float kb)
    {
        base.knockback(otherPos, kb);

        /*
        utilityAI.enabled = false;
        utilityAI.navAgent.enabled = false;

        Vector3 pos = (gameObject.transform.position - otherPos) * kb;

        LeanTween.move(gameObject, new Vector3(pos.x, pos.y + 1, pos.z), 1).setOnComplete(() =>
        {
            utilityAI.navAgent.enabled = true;
            utilityAI.enabled = true;
        });
        */
    }

    public void CallTriggerDamage()
    {
        foreach (GameObject obj in triggerList)
        {
            obj.GetComponent<Unit>().DoDamage(gameObject.transform.position, damageType);
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
