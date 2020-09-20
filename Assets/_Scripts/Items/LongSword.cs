using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    [HideInInspector] public bool attacking;

    [Header("LongSwordStats")]
    public float DoDamageAfterSec = 0.3f;


    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        // Play Idle or Walk anim
    }

    public override void attackLeftClick()
    {
        attacking = true;
    }

    public override void attackRightClick()
    {
        Debug.Log("Right Click");
        // start anim

        /*
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetBool("block", true);
        }

        if (Input.GetButtonUp("Fire2"))
        {
            animator.SetBool("block", false);
        }
        */
    }

    private void OnTriggerStay(Collider other)
    {
        if (attacking)
        {
            StartCoroutine(SwingSwordAttack(other));
        }
    }

    private IEnumerator SwingSwordAttack(Collider other)
    {
        attacking = false;
        yield return new WaitForSecondsRealtime(DoDamageAfterSec);

        //Trigger Anim
        animator.SetTrigger("hit");

        // Damage bla 
        Debug.Log("Swing Swoosh");
    }
}
