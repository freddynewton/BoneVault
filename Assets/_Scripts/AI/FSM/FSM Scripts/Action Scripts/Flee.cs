using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Flee")]
public class Flee : StateAction
{
    public float fleeRange = 5;

    public override void Action(StateMachine sm)
    {
        Debug.Log(Vector3.Distance(sm.gameObject.transform.position, PlayerController.Instance.gameObject.transform.position));

        if (Vector3.Distance(sm.gameObject.transform.position, PlayerController.Instance.gameObject.transform.position) < fleeRange)
        {
            RaycastHit hitInfo;
            Ray ray = new Ray();
            ray.direction = PlayerController.Instance.gameObject.transform.position - sm.gameObject.transform.position;
            ray.origin = sm.gameObject.transform.position;

            Physics.Raycast(ray, out hitInfo, fleeRange);

            if (hitInfo.distance < fleeRange)
            {
                sm.eUnit.navAgent.MoveToLocation((sm.gameObject.transform.position - PlayerController.Instance.transform.position) * fleeRange);
            } else
                sm.eUnit.navAgent.MoveToLocation(hitInfo.point);
        }
    }
}
