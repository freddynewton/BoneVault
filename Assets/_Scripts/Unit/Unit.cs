using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public Stats stats;

    [HideInInspector] public int currentHealth;
    [HideInInspector] public string currentState;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRend;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public FlashScreen flashScript;

    private Material baseMat;

    private float waitTicker;
    private float waitTime;

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

    public virtual void Start()
    {
        if (transform.name != "Player")
        {
            spriteRend = GetComponentInChildren<SpriteRenderer>();
            baseMat = spriteRend.material;
            rb = GetComponent<Rigidbody>();
        }

        animator = GetComponentInChildren<Animator>();
        currentHealth = stats.health;
    }

    public virtual void Update()
    {
    }

    public virtual void DoDamage(GameObject damageObj, DamageType damageType)
    {
        if (currentHealth > 0)
        {
            StartCoroutine(flashWhite(0.1f));

            if (!gameObject.CompareTag("Player")) knockback(damageObj.transform.position, damageType.knockbackForce);

            currentHealth -= damageType.damage;
            // Debug.Log("Current Health: " + currentHealth + "\nDamage: " + damageType.damage);

            if (currentHealth > 0)
            {
                hit();
            }
            else
            {
                death();
            }
        }
    }

    public virtual void hit()
    {
    }

    public virtual void death()
    {
    }

    public IEnumerator returnOppBool(bool boo, float t)
    {
        yield return new WaitForSeconds(t);
        yield return !boo;
    }

    private IEnumerator freezeGame(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }

    public virtual void knockback(Vector3 otherPos, float kb)
    {
    }

    public IEnumerator flashWhite(float time)
    {
        //spriteRend.material = Resources.Load("Material/White Shader Material") as Material;
        yield return new WaitForSeconds(time);
        StartCoroutine(freezeGame(0.035f));

        //spriteRend.material = baseMat;
    }

    public void changeAnimationState(string newState)
    {
        // prevent current animation interruption
        if (currentState == newState) return;

        // play the invoked animation
        animator.Play(newState);

        // set string to current animation as a monitor
        currentState = newState;
    }
}