using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
            spriteRend = GetComponent<SpriteRenderer>();
            baseMat = spriteRend.material;
            rb = GetComponent<Rigidbody>();
        }

        animator = GetComponent<Animator>();
        currentHealth = stats.health;
    }

    public virtual void Update()
    {
    }

    public virtual void DoDamage(Vector3 damageSrcPos, DamageType damageType)
    {
        if (currentHealth > 0)
        {
            StartCoroutine(flashWhite(0.1f));

            if (!gameObject.CompareTag("Player")) knockback(damageSrcPos, damageType.knockbackForce);

            currentHealth -= damageType.damage;
            Debug.Log("Current Health: " + currentHealth + "\nDamage: " + damageType.damage);

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

    IEnumerator freezeGame(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }

    private void knockback(Vector3 otherPos, float kb)
    {

        if (gameObject.CompareTag("Enemy"))
        {
            LeanTween.move(gameObject, (gameObject.transform.position - otherPos) * kb, 1).setEaseOutExpo();
        }

        // rb.AddForce((gameObject.transform.position - otherPos).normalized * kb, ForceMode.Impulse);
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
