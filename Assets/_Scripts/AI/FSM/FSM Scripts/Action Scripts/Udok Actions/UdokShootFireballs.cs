using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Boss/Udok/ShootFireballs")]
public class UdokShootFireballs : StateAction
{
    public float waitShootTimer = 0.2f;

    public override void Action(StateMachine sm)
    {
        if (sm.udokUnit.waitTimer(waitShootTimer))
        {
            GameObject proj = sm.udokUnit.fireBalls[Random.Range(0, sm.udokUnit.fireBalls.Count)];
            sm.udokUnit.fireBalls.Remove(proj);
            sm.udokUnit.animator.SetTrigger("Attack");

            if (proj != null)
                proj.GetComponent<Projectile>().ShootToTarget(100f, PlayerController.Instance.transform.position - sm.transform.position);
        }
    }
}
