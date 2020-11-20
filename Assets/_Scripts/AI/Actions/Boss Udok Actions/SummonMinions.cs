using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Boss/Udok/SummonMinionsUdok")]
public class SummonMinions : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (controller.bossUdokEnemyUnit.returnLivingMinions() < controller.bossUdokEnemyUnit.minionsLivingCount && !controller.bossUdokEnemyUnit.isSpawning)
        {
            controller.bossUdokEnemyUnit.spawnMinions();
        }
    }
}