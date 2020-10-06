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
    public override void Start()
    {
        base.Start();
    }

    // Attack 
    public override void attackLeftClick(bool active)
    {
        if (!isBlocking && !isAttacking)
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

    public override callbackValue callbackDamageFnc()
    {
        if (perfectBlockActive && isBlocking)
        {
            if (currentCharges < maxCharges) setCharges(currentCharges + 1);

            return callbackValue.SUCCESS;
        }
        else if (!perfectBlockActive && isBlocking)
        {
            return callbackValue.NOTHING;
        }

        return callbackValue.FAILURE;
    }

    public void blockCharge()
    {
        perfectBlockActive = false;
    }

    private void Update()
    {
        idle();

    }

    public void idle()
    {
        if (!isAttacking && !isBlocking && PlayerController.Instance.move == Vector3.zero) changeAnimationState("Idle");
        else if (!isAttacking && !isBlocking && PlayerController.Instance.move != Vector3.zero) changeAnimationState("Walk");
    }

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

    private void setCharges(int value)
    {
        currentCharges = value;
        UiManager.Instance.swordUi.setCharge(value);
        Debug.Log("Current charges: " + currentCharges);
    }

    public override void callOnEquip(bool isSpawned)
    {
        
    }

    private void OnAttackComplete()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layermaskInclude(other.gameObject.layer) && !hitObjects.Contains(other.gameObject)) hitObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (layermaskInclude(other.gameObject.layer) && hitObjects.Contains(other.gameObject)) hitObjects.Remove(other.gameObject);
    }
}
