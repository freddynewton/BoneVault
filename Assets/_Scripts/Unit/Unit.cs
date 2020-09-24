using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public Stats stats;
    
    [HideInInspector] public int currentHealth;
    [HideInInspector] public SpriteRenderer spriteRend;
    [HideInInspector] public Rigidbody rb;

    private Material baseMat;


    public virtual void Start()
    {
        if(transform.name != "Player") {
            spriteRend = GetComponent<SpriteRenderer>();
            baseMat = spriteRend.material;
            rb = GetComponent<Rigidbody>();
        }

        currentHealth = stats.health;
    }

    public virtual void Update()
    {

    }

    public virtual void DoDamage(Vector3 damageSrcPos ,int damage)
    {
        StartCoroutine(flashWhite(0.1f));

        knockback(damageSrcPos, 100);

        currentHealth -= damage;
        Debug.Log("Current Health: " + currentHealth + "\nDamage: " + damage);

        if (currentHealth > 0)
        {
            // Hit anim
        }
        else
        {
            death();
        }

        /*
        if (bulletSettings.screenShakeSetting.screenShakeOnHitCharacter)
            CameraHandler.Instance.CameraShake(bulletSettings.screenShakeSetting.duration, bulletSettings.screenShakeSetting.intensitivität, bulletSettings.screenShakeSetting.dropOffTime);
        */
    }

    public virtual void death()
    {
        // Death Anim 
        Destroy(gameObject);
    }

    IEnumerator freezeGame(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }

    private void knockback(Vector3 otherPos, float kb)
    {
        rb.AddForce((gameObject.transform.position - otherPos).normalized * kb, ForceMode.Impulse);
    }

    public IEnumerator flashWhite(float time)
    {
        //spriteRend.material = Resources.Load("Material/White Shader Material") as Material;
        yield return new WaitForSeconds(time);
        StartCoroutine(freezeGame(0.035f));

        //spriteRend.material = baseMat;
    }
}
