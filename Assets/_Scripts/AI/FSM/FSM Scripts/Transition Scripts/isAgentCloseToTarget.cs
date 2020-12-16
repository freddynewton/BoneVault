using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transitions/isAgentCloseToTarget")]
public class isAgentCloseToTarget : Transition
{
    public override bool CheckTransition(StateMachine sm)
    {
        return Vector3.Distance(sm.gameObject.transform.position, PlayerController.Instance.transform.position) > sm.eUnit.navAgent.agent.stoppingDistance;
    }
}
