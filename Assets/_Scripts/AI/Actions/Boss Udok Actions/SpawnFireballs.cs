using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Boss/Udok/SpawnFireballsBossUdok")]
public class SpawnFireballs : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (controller.bossUdokEnemyUnit.fireBalls.Count < controller.bossUdokEnemyUnit.maxFireBalls && controller.unit.waitTimer(3f))
        {
            controller.bossUdokEnemyUnit.fireBalls.Add(Instantiate<GameObject>(controller.bossUdokEnemyUnit.fireBallPF, controller.gameObject.transform.position + new Vector3(0, 2, 2), Quaternion.identity, controller.gameObject.transform));
            Projectile proj = controller.bossUdokEnemyUnit.fireBalls[controller.bossUdokEnemyUnit.fireBalls.Count - 1].GetComponent<Projectile>();
            proj.circleAroundObj = controller.gameObject;
            proj.bossUdok = controller.bossUdokEnemyUnit;
            proj.startCircleAround();
        }
    }
}
