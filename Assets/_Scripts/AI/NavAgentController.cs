using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{
    public struct rayDot
    {
        public Ray ray;
        public RaycastHit hitInfo;
        public float dotValue;
    }

    [Header("Agent Settings")]
    public NavMeshAgent agent;
    [HideInInspector] public EnemyUnit unit;

    private NavMeshPath navMeshPath;
    private const int MAX_ITERATIONS = 100;
    private Vector3 oldTargetPos;

    [HideInInspector] public int layerMask;

    // rays
    private int rayAmount = 72;
    [HideInInspector] public List<rayDot> rays = new List<rayDot>();

    [Header("Behavior settings")]
    public aiMovementBehavior moveBehavior;
    [Range(0f, 20f)] public float neighbourRadius = 10f;
    [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;
    [Range(0f, 10f)] public float stoppingDistance = 2f;
    [Range(0f, 10f)] public float driveFactor = 3f;

    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    private void Start()
    {
        // Set up
        layerMask = LayerMask.GetMask("Player, Default, Enemy, Ground");

        squareMaxSpeed = unit == null ? 3 : unit.baseStats.moveSpeed * unit.baseStats.moveSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }

    public void getPath(Vector3 targetPos)
    {
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(targetPos, out navMeshHit, 20, -1);

        // Debug.DrawLine(targetPos, navMeshHit.position, Color.green);

        NavMeshPath tmp = navMeshPath == null ? new NavMeshPath() : navMeshPath;
        NavMesh.CalculatePath(transform.position, navMeshHit.position, -1, tmp);

        if (tmp.status == NavMeshPathStatus.PathComplete && navMeshHit.position != null) oldTargetPos = navMeshHit.position;
        else NavMesh.CalculatePath(transform.position, oldTargetPos, -1, tmp);

        navMeshPath = tmp;
    }

    private void Update()
    {
        updateMovement();
    }

    private void updateMovement()
    {
        rays.Clear();
        rays = GetRayHitObjects();

        Vector3 move = moveBehavior.CalculateMove(this);

        move *= driveFactor;

        if (move.sqrMagnitude > squareMaxSpeed) move = move.normalized * (unit == null ? 3 : unit.baseStats.moveSpeed);

        Move(move);
    }

    private void Move(Vector3 velocity)
    {

        transform.forward = new Vector3(velocity.x, 0, velocity.z);
        transform.position += new Vector3(velocity.x, 0, velocity.z) * Time.deltaTime;
    }

    private List<rayDot> GetRayHitObjects()
    {
        List<rayDot> rd = new List<rayDot>();

        float angle = 0;

        // Vector3 nextCornerDirection = (navMeshPath.corners[1] - transform.position).normalized;

        for (int i = 0; i < rayAmount; i++)
        {
            // Create rayDot
            rayDot r = new rayDot();

            // Rotate Vector
            angle = 360 / rayAmount * i;
            Vector3 avoidanceDirection = (Quaternion.AngleAxis(angle, Vector3.up) * transform.forward).normalized;

            // Init ray + yAxis Offset
            Ray ray = new Ray(transform.position + new Vector3(0, 0.3f, 0), avoidanceDirection);

            // Store ray in Array
            r.ray = ray;

            // FYI: Dot Product Vectors need to be normalized
            // r.dotValue = Vector3.Dot(nextCornerDirection, avoidanceDirection);

            // Raycast
            Physics.Raycast(ray, out r.hitInfo, neighbourRadius, -1, QueryTriggerInteraction.UseGlobal);

            // Substract normalized distance to dot product
            r.dotValue -= r.hitInfo.distance / neighbourRadius;

            // Debug ray
            Debug.DrawRay(r.ray.origin, r.ray.direction * r.dotValue, Color.green);

            // add r to list
            rd.Add(r);
        }

        return rd;
    }

    #region steering behavior try
    public void moveSteeringTarget(Vector3 targetPos)
    {
        // Resets move velocity from rigidbody
        unit.rb.isKinematic = false;
        unit.rb.velocity = Vector3.zero;
        unit.rb.angularVelocity = Vector3.zero;

        // Apply new Velocity 
        unit.rb.AddForce(returnSteeringDirection(targetPos) * 100 * Time.deltaTime, ForceMode.Impulse);
    }

    private Vector3 returnSteeringDirection(Vector3 targetPos)
    {
        // Clear List
        rays.Clear();

        float angle = 0;

        // Get new Path
        getPath(targetPos);

        // Lets get desired direction
        Vector3 nextCornerDirection = (navMeshPath.corners[1] - transform.position).normalized;

        // Get all rays
        for (int i = 0; i < rayAmount; i++)
        {
            // Create rayDot
            rayDot r = new rayDot();

            // Rotate Vector
            angle = 360 / rayAmount * i;
            Vector3 avoidanceDirection = (Quaternion.AngleAxis(angle, Vector3.up) * transform.forward).normalized;

            // Init ray + yAxis Offset
            Ray ray = new Ray(transform.position + new Vector3(0, 0.3f, 0), avoidanceDirection);

            // Store ray in Array
            r.ray = ray;

            // FYI: Dot Product Vectors need to be normalized
            r.dotValue = Vector3.Dot(nextCornerDirection, avoidanceDirection);

            // Raycast
            Physics.Raycast(ray, out r.hitInfo, neighbourRadius, -1, QueryTriggerInteraction.UseGlobal);

            // Substract normalized distance to dot product
            r.dotValue -= r.hitInfo.distance / neighbourRadius;

            // Debug ray
            Debug.DrawRay(r.ray.origin, r.ray.direction * r.dotValue, Color.green);

            // add r to list
            rays.Add(r);
        }

        // Find Ray with the highest Dot value
        rayDot mainDirection = returnHighestDotValue(rays.ToArray());

        // Debug Ray
        Debug.DrawRay(mainDirection.ray.origin, mainDirection.ray.direction * mainDirection.dotValue, Color.red);

        return mainDirection.ray.direction;
    }

    private rayDot returnHighestDotValue(rayDot[] rays)
    {
        rayDot rd = new rayDot();
        float dot = 0;

        foreach (rayDot r in rays)
        {
            if (r.dotValue >= dot)
            {
                dot = r.dotValue;
                rd = r;
            }
        }
        return rd;
    }

    #endregion

    public void MoveToLocation(Vector3 targetPos)
    {
        getPath(targetPos);

        if (navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
            agent.destination = oldTargetPos;

            // moveSteeringTarget(targetPos);
        }
        else stopAgent();
    }

    public void StayInSight(Vector3 targetPos, float randomPointRange, float minDistanceToNewPoint, float navMeshSamplePositionRange)
    {
        // Check if Player is in Sight
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, targetPos - gameObject.transform.position, out hit);

        // Check if Player is in Sight and agent is not moving
        if (!hit.transform.CompareTag("Player") && agent.velocity == Vector3.zero) ;
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
        agent.speed = unit == null ? 3 : unit.baseStats.moveSpeed;
        agent.stoppingDistance = unit == null ? 3 : unit.enemyStats.stoppingDistance;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(navMeshPath.corners[1], 2);

        /*
        if (navMeshPath != null)
        {
            foreach (Vector3 p in navMeshPath.corners)
            {
                // Gizmos.color = GizmoColor;
                Gizmos.DrawWireSphere(p, 1);
            }
        }
        */
    }
}