using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Boss/Udok/ShootFireBall")]
public class ShootFireball : ActionAI
{
    public float waitShootTimer = 5f;

    public override void use(UtilityAIHandler controller)
    {
        Debug.Log("Shoot");

        if (controller.bossUdokEnemyUnit.waitTimer(waitShootTimer))
        {
            GameObject proj = controller.bossUdokEnemyUnit.fireBalls[Random.Range(0, controller.bossUdokEnemyUnit.fireBalls.Count)];
            controller.bossUdokEnemyUnit.fireBalls.Remove(proj);
            proj.GetComponent<Projectile>().ShootToTarget(PlayerController.Instance.transform.position);
        }
    }
}
