using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    [Header("LongSwordStats")]
    public float doDamageAfterSec = 0.3f;
    
    [HideInInspector] public float animationLength;


    public override void Start()
    {
        base.Start();
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking)
        {
            StartCoroutine(SwingSwordAttack(other));
        }
    }

    private IEnumerator SwingSwordAttack(Collider other)
    {
        yield return new WaitForSecondsRealtime(doDamageAfterSec);
    }

    public override void attackLeftClick() {
        if (!isBlocking) {
            int randomInt = Random.Range(1, 3);
            if (randomInt == 1) changeAnimationState("Attack1");
            else changeAnimationState("Attack2");

        }
        Invoke("attackComplete", 0.7f);
    }
}
