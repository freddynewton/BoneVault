using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator animator;
    public LayerMask ignoreRayCastMask;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public GameObject circleAroundObj;
    [HideInInspector] public BossUdokEnemyUnit bossUdok;
    [HideInInspector] public bool isCirclingAround = false;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool isHittingEnemies = false;

    private List<int> ltIds = new List<int>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ShootToTarget(float strength, Vector3 dir)
    {
        // Stop Tweens
        isCirclingAround = false;
        stopAllTweens();

        // Add Force
        rb.AddForce(dir * Time.deltaTime * strength, ForceMode.Impulse);
        
        /*ltIds.Add(LeanTween.move(gameObject, pos, time).setEaseInBack().setOnComplete(() =>
        {
            DestroyProj();
        }).id);
        */
    }



    private void Update()
    {
        if (isCirclingAround)
        {
            gameObject.transform.RotateAround(circleAroundObj.transform.position, Vector3.up, 30 * Time.deltaTime);
        }
    }

    public void stopAllTweens()
    {
        foreach(int i in ltIds)
        {
            LeanTween.cancel(i, false);
        }

        ltIds.Clear();
    }

    public void startCircleAround()
    {
        isCirclingAround = true;

        ltIds.Add(LeanTween.moveLocalY(gameObject, 2.5f, 1.3f).setEaseInOutQuad().setLoopPingPong().id);
    }

    public void DestroyProj()
    {
        stopAllTweens();
        animator.SetTrigger("Explode");
        Destroy(gameObject, 0.333f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
        }
        else if (other.CompareTag("Enemy") && isHittingEnemies)
        {
            other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
            DestroyProj();
        }
    }
}
