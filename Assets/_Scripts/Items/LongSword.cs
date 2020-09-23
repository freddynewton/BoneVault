using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    [Header("LongSwordStats")]
    public float doDamageAfterSec = 0.3f;

    [HideInInspector] public float animationLength;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isBlocking;

    public Collider Collider;


    public override void Start()
    {
        base.Start();
        Collider = GetComponent<BoxCollider>();
        Collider.enabled = false;
    }

    public override void attackLeftClick(bool active)
    {
        if (!isBlocking)
        {
            isAttacking = active;
            int randomInt = Random.Range(0, 3);

            switch (randomInt)
            {
                case 0:
                    changeAnimationState("Attack1");
                    break;
                case 1: changeAnimationState("Attack2");
                    break;
                case 2: changeAnimationState("Attack3");
                    break;
            }
        }

        StartCoroutine(SwingSwordAttack());
    }

    public override void attackRightClick(bool active)
    {
        isBlocking = active;

        if (isBlocking) changeAnimationState("Block1");
        else changeAnimationState("Block2");
    }

    private void Update()
    {
        idle();
        Debug.Log(isAttacking);
    }

    public void idle()
    {
        if (!isAttacking && !isBlocking && PlayerController.Instance.move == Vector3.zero) changeAnimationState("Idle");
        else if (!isAttacking && !isBlocking && PlayerController.Instance.move != Vector3.zero) changeAnimationState("Walk");
    }

    private IEnumerator SwingSwordAttack()
    {
        isAttacking = false;
        changeAnimationState("Idle");
        yield return new WaitForSecondsRealtime(doDamageAfterSec);
        Collider.enabled = true;
        yield return new WaitForFixedUpdate();
        Collider.enabled = false;
    }

    

    private void OnTriggerStay(Collider other)
    {

    }
}
