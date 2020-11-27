using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator animator;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public GameObject projectileSource;
    [HideInInspector] public BossUdokEnemyUnit bossUdok;
    [HideInInspector] public bool isCirclingAround = false;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool isHittingEnemies = false;

    private List<int> ltIds = new List<int>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        Invoke("DestroyProj", 20);
    }

    public void ShootToTarget(float strength, Vector3 dir)
    {
        // Stop Tweens and Movement
        isCirclingAround = false;
        stopAllTweens();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Add Force
        rb.AddForce(dir * Time.deltaTime * strength, ForceMode.Impulse);
    }

    private void Update()
    {
        if (isCirclingAround)
        {
            gameObject.transform.RotateAround(projectileSource.transform.position, Vector3.up, 30 * Time.deltaTime);
        }
    }

    public void stopAllTweens()
    {
        foreach (int i in ltIds)
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
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        stopAllTweens();
        animator.SetTrigger("Explode");
        Destroy(gameObject, 0.333f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (other.GetComponent<DestroyableEntity>())
            {
                other.GetComponent<DestroyableEntity>().interact();
            }
        }

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
        }
        else if (other.CompareTag("Enemy") && isHittingEnemies)
        {
            other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
            DestroyProj();
        } else
        {
           // DestroyProj();
        }
    }
}