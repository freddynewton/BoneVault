using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/StayInSight")]
public class StayInSight : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        controller.unit.navAgent.StayInSight(PlayerController.Instance.transform.position);
    }
}
