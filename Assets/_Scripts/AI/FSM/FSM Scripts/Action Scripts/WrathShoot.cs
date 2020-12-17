using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/WrathShoot")]
public class WrathShoot : StateAction
{
    public override void Action(StateMachine sm)
    {
        if (sm.eUnit.waitTimer(sm.eUnit.enemyStats.attackRateMin) && sm.wrathUnit.currentProj == null)
        {
            sm.wrathUnit.currentProj = Instantiate(sm.wrathUnit.projectilePF, sm.wrathUnit.transform.position + Vector3.up + sm.wrathUnit.GFX.transform.forward, Quaternion.identity, null);
            Debug.DrawLine(sm.wrathUnit.transform.forward, PlayerController.Instance.transform.position - sm.wrathUnit.transform.forward);
            sm.wrathUnit.currentProj.transform.localScale = Vector3.zero;
            Projectile proj = sm.wrathUnit.currentProj.GetComponent<Projectile>();
            proj.projectileSource = sm.gameObject;
            proj.damageType = sm.eUnit.damageType;

            sm.eUnit.animator.SetTrigger("Attack");

            LeanTween.scale(sm.wrathUnit.currentProj, Vector3.one, 1.5f).setEaseInBounce().setOnComplete(() => {
                proj.ShootToTarget(100f, PlayerController.Instance.transform.position - sm.transform.position);
            });
        }
    }
}
