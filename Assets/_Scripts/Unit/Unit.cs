using System.Collections;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Unit Settings")]
    public Stats baseStats;

    [HideInInspector] public int currentHealth;
    [HideInInspector] public string currentState;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRend;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public FlashScreen flashScript;

    protected Material baseMat;

    public abstract void hit();
    public abstract void death();

    public virtual void Start()
    {
        if (GetComponentInChildren<Animator>()) animator = GetComponentInChildren<Animator>();
        currentHealth = baseStats.maxHealth;
    }

    public virtual void DoDamage(GameObject damageObj, DamageType damageType)
    {
        if (currentHealth > 0)
        {
            knockback(damageObj.transform.position, damageType.knockbackForce);

            currentHealth -= damageType.damage;
            
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

    public IEnumerator freezeGame(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }

    public void knockback(Vector3 otherPos, float kb)
    {
        /* TODO Implement knockback */
    }
}