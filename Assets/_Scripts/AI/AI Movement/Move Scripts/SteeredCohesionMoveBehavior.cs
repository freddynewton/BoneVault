using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Move Behavior/Steered Cohesion Move")]
public class SteeredCohesionMoveBehavior : aiMovementBehavior
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(NavAgentController controller)
    {
        // if no neighbors, return no adjustment
        if (controller.rays.Count == 0) return Vector3.zero;

        // add all points together and average
        Vector3 cohesionMove = Vector3.zero;

        foreach (NavAgentController.rayDot item in controller.rays) cohesionMove += item.hitInfo.point;
        cohesionMove /= controller.rays.Count;

        // create offset from agent position
        cohesionMove -= controller.transform.position;

        // Smooth
        cohesionMove = Vector3.SmoothDamp(controller.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}
