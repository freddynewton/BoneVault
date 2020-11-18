using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;


public class PlayerUnit : Unit
{
    [HideInInspector] public float currentStamina;
    [HideInInspector] public bool isStaminaReg = true;

    public PlayerUpgradeHandler upgradeHandler;

    public float timeToRegStaminaAfterHitZero = 2f;
    private bool foolStamina;

    public override void Start()
    {
        base.Start();
        currentStamina = stats.stamina;
    }

    public void updateStamina()
    {
        if (!foolStamina && isStaminaReg && currentStamina <= stats.stamina + upgradeHandler.maxStaminaUpgrade)
        {
            setStamina(Time.deltaTime * stats.staminaRate * upgradeHandler.staminaRatePercentageUpgrade);
        }
    }

    public void setStamina(float amount)
    {
        currentStamina += amount;

        if (currentStamina > stats.stamina + upgradeHandler.maxStaminaUpgrade) currentStamina = stats.stamina + upgradeHandler.maxStaminaUpgrade;
        else if (currentStamina < 0)
        {
            foolStamina = true;
            currentStamina = 0;
            Invoke("changeFoolStamina", timeToRegStaminaAfterHitZero);
        }

        // TODO CALL UI UPDATE FUNCTION
        UiManager.Instance.setStamina();
    }

    private void changeFoolStamina()
    {
        foolStamina = false;
    }

    public override void Update()
    {
        base.Update();
        updateStamina();
    }

    public override void death()
    {
        base.death();
        // TODO Player Hit effect & "Game over" Scene 
    }

    public override void hit()
    {
        base.hit();

        // Do Player hit effect
        UiManager.Instance.flashScreen.flashScreen(1);

    }

    public override void DoDamage(GameObject damageObj, DamageType damageType)
    {
        Weapon.callbackValue v = Inventory.Instance.currWeaponScript.callbackDamageFnc();

        switch (v)
        {
            case Weapon.callbackValue.FAILURE:
                base.DoDamage(damageObj, damageType);
                UiManager.Instance.setHealth();
                stopPorjectile(damageObj, damageType);
                break;

            case Weapon.callbackValue.SUCCESS:
                // Projectile Throwback
                if (damageObj.GetComponent<Projectile>()) projectileThrowback(damageObj, damageType);
                break;

            case Weapon.callbackValue.NOTHING:
                if (currentStamina - (damageType.damage * 3) < 0)
                {
                    base.DoDamage(damageObj, damageType);
                    UiManager.Instance.setHealth();
                }
                setStamina(-damageType.damage * 3);

                stopPorjectile(damageObj, damageType);
                break;
        }
    }

    public void stopPorjectile(GameObject damageObj, DamageType damage)
    {
        if (damageObj.GetComponent<Projectile>())
        {
            Projectile projectile = damageObj.GetComponent<Projectile>();
            projectile.DestroyProj();
        }
    }

    public void projectileThrowback(GameObject damageObj, DamageType damageType)
    {
        Debug.Log("Projectile Throwback");
        LeanTween.cancel(gameObject, false);
        Projectile proj = damageObj.GetComponent<Projectile>();
        proj.isHittingEnemies = true;

        Vector3 dir = (damageObj.transform.position - proj.circleAroundObj.transform.position);
        
        proj.ShootToTarget(200, dir);
    }

    public void setHealthPlayer(int amount)
    {
        currentHealth += amount;
        if (currentHealth > stats.health + upgradeHandler.maxHealthUpgrade) currentHealth = stats.health;

        UiManager.Instance.setHealth();
    }
}
