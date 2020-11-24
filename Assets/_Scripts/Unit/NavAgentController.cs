using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{
    public NavMeshAgent agent;
    [HideInInspector] public Unit unit;

    [Header("Raycasting")]
    private const int MAX_ITERATIONS = 1000;
    public LayerMask layerMask;

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

    public void StayInSight(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();

        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, targetPos - gameObject.transform.position, out hit, layerMask);

        if (!hit.transform.CompareTag("Player"))
        {
            Vector3 target = hit.point;

            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                Vector3 randomPoint = Random.onUnitSphere * unit.stats.maxRange;

                RaycastHit _hit;
                Physics.Raycast(randomPoint, targetPos - gameObject.transform.position, out _hit, layerMask);

                if (_hit.transform.CompareTag("Player"))
                {
                    target = _hit.point;
                    break;
                }
            }

            agent.CalculatePath(target, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.isStopped = false;
                agent.destination = targetPos;
            }
            else stopAgent();
        }
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