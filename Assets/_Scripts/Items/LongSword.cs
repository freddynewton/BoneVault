using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    [Header("LongSwordStats")]
    public float doDamageAfterSec = 0.3f;


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
}
