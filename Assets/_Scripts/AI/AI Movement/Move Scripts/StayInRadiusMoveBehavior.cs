using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Move Behavior/StayInRadiusMoveBehavior")]
public class StayInRadiusMoveBehavior : aiMovementBehavior
{
    // public Vector3 center;
    public float radius = 15f;

    public override Vector3 CalculateMove(NavAgentController controller)
    {
        Vector3 centerOffset = PlayerController.Instance.transform.position - controller.transform.position;
        float t = centerOffset.magnitude / radius;

        if (t < 0.9f) return Vector3.zero;

        return centerOffset * t * t;
    }
}
