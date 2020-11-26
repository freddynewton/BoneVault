using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Wrath/WrathShooting")]
public class WrathShootingAction : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (controller.wrathEnemyUnit.waitTimer(controller.unit.stats.attackRate) && controller.wrathEnemyUnit.currentProj == null)
        {
            controller.wrathEnemyUnit.currentProj = Instantiate(controller.wrathEnemyUnit.projectilePF, controller.wrathEnemyUnit.projectileShootingPos.transform.position, Quaternion.identity, controller.gameObject.transform);
            controller.wrathEnemyUnit.currentProj.transform.localScale = Vector3.zero;
            Projectile proj = controller.wrathEnemyUnit.currentProj.GetComponent<Projectile>();
            proj.projectileSource = controller.gameObject;
            proj.damageType = controller.unit.damageType;

            controller.unit.animator.SetTrigger("Attack");

            LeanTween.scale(controller.wrathEnemyUnit.currentProj, Vector3.one, 1.5f).setEaseInBounce().setOnComplete(() => {
                proj.ShootToTarget(100f, PlayerController.Instance.transform.position - controller.transform.position);
            });
        }
    }
}
