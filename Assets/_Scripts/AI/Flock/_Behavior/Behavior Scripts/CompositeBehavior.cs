using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu( menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    [Serializable] public struct flockBehaviorWeights
    {
        public FlockBehavior behavior;
        public float Weight;
    }

    public flockBehaviorWeights[] behaviors;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // set up move
        Vector3 move = Vector3.zero;

        // Iterate through behaviors
        foreach (flockBehaviorWeights item in behaviors)
        {
            Vector3 partialMove = item.behavior.CalculateMove(agent, context, flock) * item.Weight;

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
