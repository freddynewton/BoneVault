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
        if (!isBlocking) changeAnimationState("Attack1");
        animationLength = animator.GetCurrentAnimatorStateInfo(0).length; // diese Zeile funktioniert nicht, daher ist unten hardcoded die Dauer drin

        Invoke("attackComplete", 0.35f); // hier müsste eig animationLength rein als zweiter Parameter
    }
}
