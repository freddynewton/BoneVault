using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/SkeletonAttack")]

public class SkeletonAttack : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (controller.unit.waitTimer(controller.unit.stats.attackRate))
        {
            controller.unit.animator.SetTrigger("Attack");
            controller.unit.playRandomSFX(controller.unit.attackSFX);
        }
    }
}