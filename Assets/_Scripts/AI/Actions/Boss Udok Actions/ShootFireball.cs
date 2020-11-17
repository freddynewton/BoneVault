using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Boss/Udok/ShootFireBall")]
public class ShootFireball : ActionAI
{
    public float waitShootTimer = 0.8f;

    public override void use(UtilityAIHandler controller)
    {
        if (controller.bossUdokEnemyUnit.waitTimer(waitShootTimer))
        {
            GameObject proj = controller.bossUdokEnemyUnit.fireBalls[Random.Range(0, controller.bossUdokEnemyUnit.fireBalls.Count)];
            controller.bossUdokEnemyUnit.fireBalls.Remove(proj);
            proj.GetComponent<Projectile>().ShootToTarget(100f, controller.transform.position - PlayerController.Instance.transform.position);
        }
    }
}
