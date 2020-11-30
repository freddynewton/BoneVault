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

    public void StayInSight(Vector3 targetPos, float yAxisOffset)
    {
        // Check if Player is in Sight
        RaycastHit hit;

        // Check if Player is in Sight and agent is not moving
        if (Physics.Raycast(gameObject.transform.position, targetPos - gameObject.transform.position, out hit) && !hit.transform.CompareTag("Player") && agent.velocity == Vector3.zero) ;
        {
            // Find Random Point in Range
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                Vector3 _tmp = gameObject.transform.position + (Random.insideUnitSphere * 30);

                // Check distance to ground
                RaycastHit _hitYaxis;
                Physics.Raycast(_tmp, Vector3.down, out _hitYaxis);

                // Change new y Axis Value
                _tmp = new Vector3(_tmp.x, _hitYaxis.point.y + yAxisOffset, _tmp.z);

                //Raycast again from new Random Point
                Physics.Raycast(_tmp, targetPos - _tmp, out hit);

                // Check if Player is in Sight from new Pos
                if (hit.transform.CompareTag("Player") && Vector3.Distance(gameObject.transform.position, _tmp) >= 5 && agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    Debug.Log(hit.transform.tag);

                    //Move Agent
                    MoveToLocation(_tmp);

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