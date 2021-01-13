using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/WanderAround")]
public class WanderAroundAction : StateAction
{
    public override void Action(StateMachine sm)
    {
        sm.eUnit.navAgent.wanderAround(15, 40, Random.Range(1, 4));
    }
}
