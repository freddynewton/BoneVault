using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/SkeletonAttack")]

public class SkeletonAttack : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (controller.unit.waitTimer(controller.unit.enemyStats.attackRateMin))
        {
            controller.unit.animator.SetTrigger("Attack");
            SoundManager.Instance.playRandomSFX(controller.unit.attackSFX, controller.unit.audioSource, 0.8f, 1.2f);
        }
    }
}