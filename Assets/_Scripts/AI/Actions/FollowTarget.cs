using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/FollowTarget")]
public class FollowTarget : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (Vector3.Distance(controller.gameObject.transform.position, PlayerController.Instance.gameObject.transform.position) < controller.unit.enemyStats.stoppingDistance) return;

        if (controller.unit.navAgent.agent != null) controller.unit.navAgent.MoveToLocation(PlayerController.Instance.transform.position);
    }
}