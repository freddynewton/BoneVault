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


    public override void Start()
    {
        base.Start();
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
        //Invoke("attackComplete", 0.7f);
    }

    public override void attackRightClick(bool active)
    {
        isBlocking = active;
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

    private IEnumerator SwingSwordAttack(Collider other)
    {
        isAttacking = false;
        yield return new WaitForSecondsRealtime(doDamageAfterSec);

    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking)
        {
            StartCoroutine(SwingSwordAttack(other));
        }
    }
}
