using System.Collections.Generic;
using UnityEngine;

public class BossUdokEnemyUnit : EnemyUnit
{
    [Header("Boss Settings")]
    public BossRoom bossRoom;

    [Header("Fire Balls")]
    public List<GameObject> fireBalls = new List<GameObject>();

    public int maxFireBalls;
    public GameObject fireBallPF;
    public GameObject[] fireballFlyPos;

    [Header("SpawnedMinions")]
    public List<EnemyUnit> spawnedMinions = new List<EnemyUnit>();

    public int minionsLivingCount = 3;

    public override void Start()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();
        currentHealth = stats.health;
        utilityAI = GetComponent<UtilityAIHandler>();

        LeanTween.moveLocalY(gameObject, gameObject.transform.position.y - 1, 2.5f).setEaseInOutQuad().setLoopPingPong();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void knockback(Vector3 otherPos, float kb)
    {
        base.knockback(otherPos, kb);
    }

    public override void hit()
    {
        base.hit();
    }

    public override void setWalkingAnimation()
    {
        // Nothing
    }

    public override void DoDamage(GameObject damageObj, DamageType damageType)
    {
        base.DoDamage(damageObj, damageType);
    }

    public override void death()
    {
        foreach (GameObject fireball in fireBalls)
        {
            fireball.GetComponent<Projectile>().DestroyProj();
        }

        Inventory.Instance.setBones(Random.Range(0, 10));
        base.death();
        bossRoom.portalDoor.openDoor();
    }
}