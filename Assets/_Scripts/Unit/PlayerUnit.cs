using UnityEngine;

public class PlayerUnit : Unit
{
    public PlayerUpgradeHandler upgradeHandler;
    public AudioClip [] hitSFX;
    public float timeToRegStaminaAfterHitZero = 2f;

    private bool foolStamina;
    [HideInInspector] public float currentStamina;
    [HideInInspector] public bool isStaminaReg = true;
    [HideInInspector] public AudioSource randomSound;

    public override void Start()
    {
        base.Start();
        currentStamina = stats.stamina;
        randomSound = GetComponentInChildren<AudioSource>();
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
        UiManager.Instance.flashScreen.flashScreen(1);
        playRandomSFX(hitSFX);
        // TODO Player Hit effect & "Game over" Scene
    }

    public override void hit()
    {
        base.hit();

        // Do Player hit effect
        UiManager.Instance.flashScreen.flashScreen(1);

        playRandomSFX(hitSFX);
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

        proj.ShootToTarget(100, proj.projectileSource.transform.position - damageObj.transform.position);
    }

    public void setHealthPlayer(int amount)
    {
        currentHealth += amount;
        if (currentHealth > stats.health + upgradeHandler.maxHealthUpgrade) currentHealth = stats.health;

        UiManager.Instance.setHealth();
    }

    // SFX Handler
    private void playRandomSFX (AudioClip [] sounds) {
        randomSound.clip = sounds [Random.Range(0, sounds.Length)];
        randomSound.pitch = Random.Range(0.8f, 1.2f);

        if (randomSound != null) randomSound.Play();
    }
}