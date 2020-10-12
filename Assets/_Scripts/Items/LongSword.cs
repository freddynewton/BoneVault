using BoneVault.CameraEffects;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LongSword : Weapon
{
    [Header("LongSwordStats")]
    public float doDamageAfterSec = 0.3f;
    public int maxCharges = 3;
    private int currentCharges;

    [Header("Block Stats")]
    public float perfectBlockDuration = 5;
    public bool knockbackOnPerfectBlock = true;
    [HideInInspector] public bool perfectBlockActive;


    [HideInInspector] public float animationLength;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isBlocking;

    // INVOKE FUNCTIONS
    public void blockCharge() => perfectBlockActive = false;
    private void OnAttackComplete() => isAttacking = false;

    public override void Start()
    {
        base.Start();
    }

    // Attack 
    public override void attackLeftClick(bool active)
    {
        if (!isBlocking && !isAttacking && PlayerController.Instance.unit.currentStamina > 0)
        {
            isAttacking = active;
            int randomInt = Random.Range(0, 3);

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
    public override void attackRightClick(bool active)
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
            CameraEffects.ShakeOnce(ShakeLenght, ShakeStrength);

            return callbackValue.SUCCESS;
        }

        // Normal blocking
        else if (!perfectBlockActive && isBlocking)
        {
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
        else if (!isAttacking && !isBlocking && PlayerController.Instance.move != Vector3.zero) changeAnimationState("Walk");
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

            if (objU.currentHealth - damageType.damage <= 0) remList.Add(obj);

            // TODO Clean trash code
            int d = damageType.damage;
            damageType.damage = d + currentCharges;

            objU.DoDamage(gameObject.transform.position, damageType);

            damageType.damage = d;
            // End Trash Code
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
        Debug.Log("Current charges: " + currentCharges);
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
        } else
        {
            UiManager.Instance.weaponUI.activateSwordUI(false);
        }
    }

    // Trigger Handler
    private void OnTriggerEnter(Collider other)
    {
        if (layermaskInclude(other.gameObject.layer) && !hitObjects.Contains(other.gameObject)) hitObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (layermaskInclude(other.gameObject.layer) && hitObjects.Contains(other.gameObject)) hitObjects.Remove(other.gameObject);
    }
}
