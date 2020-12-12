using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{
    [Header("Agent Settings")]
    public NavMeshAgent agent;
    [HideInInspector] public EnemyUnit unit;

    private NavMeshPath navMeshPath;
    private const int MAX_ITERATIONS = 100;
    private Vector3 oldTargetPos;
    public Color GizmoColor;
    public int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Player, Default, Enemy, Ground");
    }

    public void getPath(Vector3 targetPos)
    {
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(targetPos, out navMeshHit, 20, -1);

        NavMeshPath tmp = navMeshPath == null ? new NavMeshPath() : navMeshPath;
        NavMesh.CalculatePath(transform.position, navMeshHit.position, -1, tmp);

        if (tmp.status == NavMeshPathStatus.PathComplete && navMeshHit.position != null) oldTargetPos = navMeshHit.position;
        else NavMesh.CalculatePath(transform.position, oldTargetPos, -1, tmp);

        navMeshPath = tmp;
    }

    public void MoveToLocation(Vector3 targetPos)
    {
        getPath(targetPos);

        if (navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
            agent.destination = oldTargetPos;
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
        unit = GetComponent<EnemyUnit>();
        agent.speed = unit.baseStats.moveSpeed;
        agent.stoppingDistance = unit.enemyStats.stoppingDistance;
    }

    private void OnDrawGizmos()
    {
        if (navMeshPath != null)
        {
            foreach (Vector3 p in navMeshPath.corners)
            {
                // Gizmos.color = GizmoColor;
                Gizmos.DrawWireSphere(p, 1);
            }
        }
        
    }
}