using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/FollowTarget")]
public class FollowTarget : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
       /* if (controller.navAgent.remainingDistance != controller.unit.stats.stoppingDistance)*/ controller.navAgent.SetDestination(PlayerController.Instance.transform.position); 
        
    }
}
