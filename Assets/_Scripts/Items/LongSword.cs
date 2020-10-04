using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LongSword : Weapon
{
    [Header("LongSwordStats")]
    public float doDamageAfterSec = 0.3f;

    [HideInInspector] public float animationLength;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isBlocking;
    public override void Start()
    {
        base.Start();
    }

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

    public override void attackRightClick(bool active)
    {

        if (!isAttacking)
        {
            isBlocking = active;
            PlayerController.Instance.unit.isStaminaReg = !active;
            int randomInt = Random.Range(1, 3);

            changeAnimationState("Block" + randomInt.ToString());
        }
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

            if (objU.currentHealth - Damage <= 0) remList.Add(obj);

            objU.DoDamage(gameObject.transform.position, Damage, knockbackForce);
        }

        foreach (GameObject obj in remList) hitObjects.Remove(obj);
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
