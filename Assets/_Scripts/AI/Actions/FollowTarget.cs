using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/FollowTarget")]
public class FollowTarget : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (Vector3.Distance(controller.gameObject.transform.position, PlayerController.Instance.gameObject.transform.position) < controller.unit.stats.stoppingDistance) return;

        if (controller.navAgent != null) controller.navAgent.SetDestination(PlayerController.Instance.transform.position);
    }
}