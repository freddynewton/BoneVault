using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Move Behavior/Alignment Move")]
public class AlignmentMoveBehavior : aiMovementBehavior
{
    public override Vector3 CalculateMove(NavAgentController controller)
    {
        // if no neighbors, maintain current alignment
        if (controller.rays.Count == 0) return controller.transform.forward;

        // add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        int nEnemy = 0;

        foreach (NavAgentController.rayDot item in controller.rays)
        {
            if (item.hitInfo.collider != null)
            {
                if (item.hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    nEnemy++;
                    alignmentMove += item.hitInfo.collider.transform.forward;
                }
            }
            
        }

        if (nEnemy > 0) alignmentMove /= nEnemy;

        return alignmentMove;
    }
}
