using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/FollowUntilStoppingDisAction")]
public class FollowUntilStoppingDisAction : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (Vector3.Distance(controller.gameObject.transform.position, PlayerController.Instance.gameObject.transform.position) < controller.navAgent.stoppingDistance)
            controller.navAgent.SetDestination(PlayerController.Instance.gameObject.transform.position);
    }
}
