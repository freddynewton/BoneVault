using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{
    public NavMeshAgent agent;
    [HideInInspector] public Unit unit;

    public void MoveToLocation(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPos, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
            agent.destination = targetPos;
        }
        else stopAgent();
    }

    public void stopAgent()
    {
        agent.isStopped = true;
    }

    private void Awake()
    {
        unit = GetComponent<Unit>();
        agent.speed = unit.stats.moveSpeed;
        agent.stoppingDistance = unit.stats.stoppingDistance;
    }
}