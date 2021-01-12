using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/WanderAround")]
public class WanderAroundAction : StateAction
{

    public override void Action(StateMachine sm)
    {
        // if (sm.isTimerDone(Random.Range(3, 9)))
        { 
            sm.eUnit.navAgent.wanderAround(15, 40);
        }
    }
}
