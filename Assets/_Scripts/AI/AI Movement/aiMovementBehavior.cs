using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class aiMovementBehavior : ScriptableObject
{
    public abstract Vector3 CalculateMove(NavAgentController controller);
}
