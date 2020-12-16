using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/StayInSightAction")]
public class StayInSightAction : StateAction
{
    public override void Action(StateMachine sm)
    {
        sm.eUnit.navAgent.StayInSight(PlayerController.Instance.transform.position, 30, 5, 5);
    }
}
