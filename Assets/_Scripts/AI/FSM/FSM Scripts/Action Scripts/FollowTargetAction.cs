using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/FollowTargetAction")]
public class FollowTargetAction : StateAction
{
    public override void Action(StateMachine sm)
    {
        sm.eUnit.navAgent.MoveToLocation(PlayerController.Instance.transform.position);
    }
}
