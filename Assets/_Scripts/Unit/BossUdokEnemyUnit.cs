using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUdokEnemyUnit : EnemyUnit
{
    public AudioClip [] summonSFX;

    [Header("Boss Settings")]
    public BossRoom bossRoom;

    [Header("Fire Balls")]
    public List<GameObject> fireBalls = new List<GameObject>();

    public int maxFireBalls;
    public GameObject fireBallPF;
    public GameObject[] fireballFlyPos;

    [Header("SpawnedMinions")]
    public List<GameObject> enemyTypes = new List<GameObject>();

    public int minionsLivingCount = 3;
    [HideInInspector] public List<EnemyUnit> spawnedMinions = new List<EnemyUnit>();
    [HideInInspector] public bool isSpawning;

    public override void Start()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();
        currentHealth = stats.maxHealth;
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
            fireball.GetComponent<Projectile>().DestroyProjectile();
        }

        Inventory.Instance.setBones(Random.Range(0, 10));
        base.death();
        bossRoom.portalDoor.openDoor();
    }

    public int returnLivingMinions()
    {
        int count = 0;

        if (spawnedMinions.Count == 0) return 0;
        foreach (EnemyUnit e in spawnedMinions)
        {
            if (e.currentHealth > 0) count++;
        }

        return count;
    }

    public void spawnMinions()
    {
        // Cancel LeanTween
        LeanTween.cancel(gameObject);
        StartCoroutine(setSpawning(true, 0));

        // Find Ground pos
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);

        // Move To Ground
        LeanTween.moveY(gameObject, hit.point.y, 1.5f).setEaseOutSine().setOnComplete(() => {

            // Play SFX and Anim
            animator.SetTrigger("Summon");
            playRandomSFX(summonSFX, GetComponent<AudioSource>());

            // Fill Missing Adds
            for (int i = returnLivingMinions(); i < minionsLivingCount; i++)
            {
                // Spawn Random Minion
                Vector3 spawnPos = bossRoom.transform.position;
                spawnPos.y = hit.point.y;
                GameObject e = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], spawnPos, Quaternion.identity, bossRoom.transform);
                spawnedMinions.Add(e.GetComponent<EnemyUnit>());
            }

            // Move up
            LeanTween.moveY(gameObject, gameObject.transform.position.y + hit.distance, 4f).setEaseOutSine().setDelay(4f).setOnComplete(() =>
            {
                StartCoroutine(setSpawning(false, 5));
                LeanTween.moveLocalY(gameObject, gameObject.transform.position.y - 1, 2.5f).setEaseInOutQuad().setLoopPingPong();
            });
        });
    }

    private IEnumerator setSpawning(bool active, float t)
    {
        yield return new WaitForSecondsRealtime(t);
        isSpawning = active;
    }
}