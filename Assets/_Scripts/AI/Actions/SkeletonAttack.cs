﻿using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/SkeletonAttack")]

public class SkeletonAttack : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (controller.unit.waitTimer(controller.unit.enemyStats.attackRateMin))
        {
            controller.unit.animator.SetTrigger("Attack");
            controller.unit.playRandomSFX(controller.unit.attackSFX, controller.unit.GetComponent<AudioSource>());
        }
    }
}