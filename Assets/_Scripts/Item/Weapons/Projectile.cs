using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator animator;
    public bool destoryInteractables = true;
    public AudioClip explodeSFX;

    [HideInInspector] public DamageType damageType;
    [HideInInspector] public GameObject projectileSource;
    [HideInInspector] public BossUdokEnemyUnit bossUdok;
    [HideInInspector] public bool isCirclingAround = false;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool isHittingEnemies = false;

    private List<int> ltIds = new List<int>();

    #region monobehaviour
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Physics.IgnoreLayerCollision(14, 15);
    }
    private void Awake()
    {
        Invoke("DestroyProj", 20);
    }
    private void Update()
    {
        if (isCirclingAround)
        {
            gameObject.transform.RotateAround(projectileSource.transform.position, Vector3.up, 30 * Time.deltaTime);
        }
    }
    #endregion

    public void ShootToTarget(float strength, Vector3 dir)
    {
        // Stop Tweens and Movement
        gameObject.transform.parent = null;
        isCirclingAround = false;
        stopAllTweens();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Add Force
        rb.AddForce(dir * Time.deltaTime * strength, ForceMode.Impulse);
    }


    public void startCircleAround()
    {
        isCirclingAround = true;

        ltIds.Add(LeanTween.moveLocalY(gameObject, 2.5f, 1.3f).setEaseInOutQuad().setLoopPingPong().id);
    }

    public void DestroyProjectile()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        AudioSource sfx = GetComponent<AudioSource>();
        sfx.clip = explodeSFX;
        sfx.loop = false;
        sfx.Play();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        stopAllTweens();
        animator.SetTrigger("Explode");

        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject && other.GetComponent<Room>() == null)
        {
            switch (other.tag)
            {
                case "Interactable":
                    WorldItem wi = other.GetComponent<WorldItem>();
                    if (wi != null && wi.isDestroyable && destoryInteractables)
                    {
                        wi.interact();
                        DestroyProjectile();
                    }
                    break;
                case "Player":
                    other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
                    break;
                case "Enemy":
                    if (isHittingEnemies)
                    {
                        other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
                        DestroyProjectile();
                    }
                    break;
                case "Weapon":
                    break;
                case "Projectile":
                    break;
                default:
                    DestroyProjectile();
                    break;
            }
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
}