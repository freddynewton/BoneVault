using UnityEngine;

public class PlayerUnit : Unit
{
    [Header("PlayerStats")]
    public PlayerStats playerStats;

    [Header("Upgrade Handler")]
    public PlayerUpgradeHandler upgradeHandler;

    [Header("SFX")]
    public AudioClip [] hitSFX;

    [Header("Stamina Settings")]
    public float timeToRegStaminaAfterHitZero = 2f;

    private bool foolStamina;
    [HideInInspector] public float currentStamina;
    [HideInInspector] public bool isStaminaReg = true;
    [HideInInspector] public AudioSource randomSound;

    private void changeFoolStamina() => foolStamina = false;

    public override void Start()
    {
        base.Start();
        currentStamina = playerStats.stamina;
        randomSound = GetComponentInChildren<AudioSource>();
    }
    public void Update()
    {
        updateStamina();
    }

    #region Stamina

    public void updateStamina()
    {
        if (!foolStamina && isStaminaReg && currentStamina <= playerStats.stamina + upgradeHandler.maxStaminaUpgrade)
        {
            setStamina(Time.deltaTime * playerStats.staminaRate * upgradeHandler.staminaRatePercentageUpgrade);
        }
    }


    #endregion

    #region Setter
    public void setHealthPlayer(int amount)
    {
        currentHealth += amount;
        if (currentHealth > baseStats.maxHealth + upgradeHandler.maxHealthUpgrade) currentHealth = baseStats.maxHealth;

        UiManager.Instance.setHealth();
    }

    public void setStamina(float amount)
    {
        currentStamina += amount;

        if (currentStamina > playerStats.stamina + upgradeHandler.maxStaminaUpgrade) currentStamina = playerStats.stamina + upgradeHandler.maxStaminaUpgrade;
        else if (currentStamina < 0)
        {
            foolStamina = true;
            currentStamina = 0;
            Invoke("changeFoolStamina", timeToRegStaminaAfterHitZero);
        }

        // TODO CALL UI UPDATE FUNCTION
        UiManager.Instance.setStamina();
    }
    #endregion

    #region Hit Controll

    public override void death()
    {
        UiManager.Instance.flashScreen.flashScreen(1, new Color(1f, 0f, 0f, 0.5f));
        SoundManager.Instance.playRandomSFX(hitSFX, randomSound, 0.8f, 1.2f);

        // TODO Player Hit effect & "Game over" Scene
        UiManager.Instance.Death();
    }

    public override void hit()
    {
        // Do Player hit effect
        UiManager.Instance.flashScreen.flashScreen(1, new Color(1f, 0f, 0f, 0.5f));

        SoundManager.Instance.playRandomSFX(hitSFX, randomSound, 0.8f, 1.2f);
    }

    public override void DoDamage(GameObject damageObj, DamageType damageType)
    {
        Weapon.callbackValue v = Inventory.Instance.currWeaponScript.callbackDamageFnc();

        switch (v)
        {
            case Weapon.callbackValue.FAILURE:
                base.DoDamage(damageObj, damageType);
                UiManager.Instance.setHealth();
                stopProjectile(damageObj, damageType);
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

                stopProjectile(damageObj, damageType);
                break;
        }
    }

    #endregion

    #region Projectile Management
    public void stopProjectile(GameObject damageObj, DamageType damage)
    {
        if (damageObj.GetComponent<Projectile>())
        {
            Projectile projectile = damageObj.GetComponent<Projectile>();
            projectile.DestroyProjectile();
        }
    }

    public void projectileThrowback(GameObject damageObj, DamageType damageType)
    {
        LeanTween.cancel(gameObject, false);
        Projectile proj = damageObj.GetComponent<Projectile>();
        proj.isHittingEnemies = true;

        proj.ShootToTarget(100, proj.projectileSource.transform.position - damageObj.transform.position);
    }
    #endregion
}