using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "AI/Move Behavior/CompositeMoveBehavior")]
public class CompositeMoveBehavior : aiMovementBehavior
{
    [Serializable]
    public struct aiMoveBehavCol
    {
        public aiMovementBehavior behavior;
        public float Weight;
    }

    public aiMoveBehavCol[] behaviors;

    public override Vector3 CalculateMove(NavAgentController controller)
    {
        // set up move
        Vector3 move = Vector3.zero;

        // Iterate through behaviors
        foreach (aiMoveBehavCol item in behaviors)
        {
            Vector3 partialMove = item.behavior.CalculateMove(controller) * item.Weight;

            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > item.Weight * item.Weight)
                {
                    partialMove.Normalize();
                    partialMove *= item.Weight;
                }

                move += partialMove;
            }
        }

        return move;
    }
}
