using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Move Behavior/Avoidance Move")]
public class AvoidanceMoveBehavior : aiMovementBehavior
{
    public override Vector3 CalculateMove(NavAgentController controller)
    {
        if (controller.rays.Count == 0) return Vector3.zero;

        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;

        foreach (NavAgentController.rayDot r in controller.rays)
        {
            if (Vector3.SqrMagnitude(r.hitInfo.point - controller.transform.position) < controller.SquareAvoidanceRadius
                && r.hitInfo.collider != null)
            {
                nAvoid++;
                avoidanceMove += controller.transform.position - r.hitInfo.point;
            }
        }

        if (nAvoid > 0) avoidanceMove /= nAvoid;

        return avoidanceMove;
    }
}
