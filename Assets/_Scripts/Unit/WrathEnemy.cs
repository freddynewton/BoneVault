using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathEnemy : EnemyUnit
{
    [Header("Wrath Settings")]
    public GameObject projectilePF;
    public GameObject projectileShootingPos;
    [HideInInspector] public GameObject currentProj;
    public GameObject GFX;

    private void Awake()
    {
        // Flying Effect
        // LeanTween.moveLocalY(gameObject, 0.3f, 4f).setEaseInOutBack().setDelay(Random.Range(0f, 2f)).setLoopPingPong();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void setWalkingAnimation()
    {
        base.setWalkingAnimation();
    }

    public override void knockback(Vector3 otherPos, float kb)
    {
        base.knockback(otherPos, kb);
    }
    public override void DoDamage(GameObject damageObj, DamageType damageType)
    {
        base.DoDamage(damageObj, damageType);
    }
    public override void death()
    {
        base.death();
    }

    public override void hit()
    {
        base.hit();
    }
}
