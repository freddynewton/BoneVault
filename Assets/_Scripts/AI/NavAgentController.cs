using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{
    public NavMeshAgent agent;
    [HideInInspector] public Unit unit;

    [Header("Raycasting")]
    private const int MAX_ITERATIONS = 100;

    public int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Player, Default, Enemy, Ground");
    }

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

    public void StayInSight(Vector3 targetPos, float randomPointRange, float minDistanceToNewPoint, float navMeshSamplePositionRange)
    {
        // Check if Player is in Sight
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, targetPos - gameObject.transform.position, out hit);

        // Check if Player is in Sight and agent is not moving
        if (!hit.transform.CompareTag("Player") && agent.velocity == Vector3.zero);
        {
            // Find Random Point in Range
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                Vector3 _tmp = gameObject.transform.position + (Random.insideUnitSphere * randomPointRange);

                // Raycast again from new Random Point
                Physics.Raycast(_tmp, targetPos - _tmp, out hit);

                // Check if Player is in Sight from new Pos
                if (hit.transform.CompareTag("Player") && Vector3.Distance(gameObject.transform.position, _tmp) >= minDistanceToNewPoint && agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    // Get closest Navmeshpoint from new Point
                    NavMeshHit navHit;
                    if (NavMesh.SamplePosition(_tmp, out navHit, navMeshSamplePositionRange, NavMesh.AllAreas))
                    {
                        // Move Agent
                        MoveToLocation(navHit.position);
                    }

                    // End for Loop
                    break;
                }
            }
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