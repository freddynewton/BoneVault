using BoneVault.CameraEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandSword : MeleeWeapon
{
    [Header("One Hand Sword Settings")]
    public int maxCharges = 3;
    private int currentCharges;
    public float perfectBlockDuration = 5;

    public bool knockbackOnPerfectBlock = true;
    [HideInInspector] public bool perfectBlockActive;
    [HideInInspector] public float animationLength;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isBlocking;
    [HideInInspector] public AudioSource audioSource;

    public ParticleSystem sparks;
    public ParticleSystem sparksCharged;
    public AudioClip [] swingSFX;
    public AudioClip [] parrySFX;

    // INVOKE FUNCTIONS
    public void blockCharge() => perfectBlockActive = false;

    private void OnAttackComplete() => isAttacking = false;

    public override void Start()
    {
        base.Start();
        sparks.Stop();
        sparksCharged.Stop();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Attack
    public override void ability1(bool active)
    {
        if (!isBlocking && !isAttacking && PlayerController.Instance.unit.currentStamina > 0)
        {
            isAttacking = active;
            int randomInt = Random.Range(0, 3);
            SoundManager.Instance.playRandomSFX(swingSFX, audioSource, 0.8f, 1.2f);

            switch (randomInt)
            {
                case 0:
                    changeAnimationState("Attack1");
                    break;

                case 1:
                    changeAnimationState("Attack2");
                    break;

                case 2:
                    changeAnimationState("Attack3");
                    break;
            }

            PlayerController.Instance.unit.setStamina(-damageType.staminaCost);

            Invoke("OnAttackComplete", 0.7f);
            StartCoroutine(SwingSwordAttack());
        }
    }

    // Block
    public override void ability2(bool active)
    {
        if (!isAttacking)
        {
            isBlocking = active;
            PlayerController.Instance.unit.isStaminaReg = !active;
            int randomInt = Random.Range(1, 3);

            changeAnimationState("Block" + randomInt.ToString());

            if (active)
            {
                perfectBlockActive = active;
                Invoke("blockCharge", perfectBlockDuration);
            }
            else
            {
                perfectBlockActive = active;
            }
        }
    }

    /// <summary>
    /// Abstract Callback Function to handle blocking and everything else with the Player Unit
    /// </summary>
    /// <returns></returns>
    public override callbackValue callbackDamageFnc()
    {
        // Add Charge because of Perfect blocking
        if (perfectBlockActive && isBlocking)
        {
            if (currentCharges < maxCharges) setCharges(currentCharges + 1);

            // Camera Shake
            CameraEffects.ShakeOnce(ShakeLength, ShakeStrength);

            sparksCharged.Clear();
            sparksCharged.Play();
            SoundManager.Instance.playRandomSFX(parrySFX, audioSource, 0.8f, 1.2f);

            return callbackValue.SUCCESS;
        }

        // Normal blocking
        else if (!perfectBlockActive && isBlocking)
        {
            sparks.Clear();
            sparks.Play();
            SoundManager.Instance.playRandomSFX(parrySFX, audioSource, 0.8f, 1.2f);

            return callbackValue.NOTHING;
        }

        // No blocking
        return callbackValue.FAILURE;
    }

    /// <summary>
    /// Frame Ticking Update Function
    /// </summary>
    private void Update()
    {
        idle();
    }

    /// <summary>
    /// Set Walk and Idle Animations
    /// </summary>
    public void idle()
    {
        if (!isAttacking && !isBlocking && PlayerController.Instance.move == Vector3.zero) changeAnimationState("Idle");
        else if (!isAttacking && !isBlocking && !PlayerController.Instance.isSprinting && PlayerController.Instance.move != Vector3.zero) changeAnimationState("Walk");
        else if (!isAttacking && !isBlocking && PlayerController.Instance.isSprinting && PlayerController.Instance.move != Vector3.zero) changeAnimationState("Sprint");
    }

    /// <summary>
    /// Courotine that Handles the Damage after x seconds from the Sword
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwingSwordAttack()
    {
        yield return new WaitForSecondsRealtime(doDamageAfterSec);

        List<GameObject> remList = new List<GameObject>();

        foreach (GameObject obj in hitObjects)
        {
            Unit objU = obj.GetComponent<Unit>();
            if (objU != null)
            {
                if (objU.currentHealth - damageType.damage <= 0) remList.Add(obj);

                // TODO Clean trash code
                int d = damageType.damage;
                damageType.damage = d + currentCharges;

                objU.DoDamage(gameObject, damageType);

                damageType.damage = d;
                // End Trash Code
            }

            // Interact with Destroyable Item
            WorldItem wi = obj.GetComponent<WorldItem>();
            if (wi != null && wi.isDestroyable) wi.interact();
        }

        foreach (GameObject obj in remList) hitObjects.Remove(obj);

        setCharges(0);
    }

    /// <summary>
    /// Set Charges and Updates the UI
    /// </summary>
    /// <param name="value"></param>
    private void setCharges(int value)
    {
        currentCharges = value;
        UiManager.Instance.weaponUI.swordUI.setCharge(value);
        // Debug.Log("Current charges: " + currentCharges);
    }

    /// <summary>
    /// Change Sword HUD UI based on the parameters
    /// </summary>
    /// <param name="isSpawned"></param>
    public override void callOnEquip(bool isSpawned)
    {
        if (isSpawned)
        {
            UiManager.Instance.weaponUI.activateSwordUI(true);
            UiManager.Instance.weaponUI.swordUI.spawnUI(maxCharges);
            UiManager.Instance.weaponUI.swordUI.setCharge(currentCharges);
        }
        else
        {
            UiManager.Instance.weaponUI.activateSwordUI(false);
        }
    }
}