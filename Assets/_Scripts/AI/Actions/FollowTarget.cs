using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/FollowTarget")]
public class FollowTarget : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (Vector3.Distance(controller.gameObject.transform.position, PlayerController.Instance.gameObject.transform.position) < controller.unit.stats.stoppingDistance) return;

        controller.navAgent.SetDestination(PlayerController.Instance.transform.position);
    }
}
