using UnityEditor.Experimental.GraphView;
using UnityEngine.AI;

using UnityEngine;

/// <summary>
/// Action node for chasing the enemy.
/// </summary>
public class ChaseNode : Node
{
    // --- Private Fields ---
    private NavMeshAgent agent;
    private Transform enemy;

    // --- Constructor ---
    public ChaseNode(NavMeshAgent agent, Transform enemy)
    {
        this.agent = agent;
        this.enemy = enemy;
    }

    // --- Public Methods ---
    public override NodeStatus Evaluate()
    {
        agent.SetDestination(enemy.position);
        return NodeStatus.RUNNING;
    }
}

/// <summary>
/// Action node for patrolling between waypoints and idling.
/// </summary>
public class PatrolNode : Node
{
    // --- Private Fields ---
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private Transform idlePoint;
    private float idleTimeThreshold;
    private float wanderTimeThreshold;

    private int currentWaypointIndex = 0;
    private float wanderTimer = 0f;
    private float idleTimer = 0f;
    private bool isIdling = false;

    // --- Constructor ---
    public PatrolNode(NavMeshAgent agent, Transform[] waypoints, Transform idlePoint, float idleTimeThreshold, float wanderTimeThreshold)
    {
        this.agent = agent;
        this.waypoints = waypoints;
        this.idlePoint = idlePoint;
        this.idleTimeThreshold = idleTimeThreshold;
        this.wanderTimeThreshold = wanderTimeThreshold;
    }

    // --- Public Methods ---
    public override NodeStatus Evaluate()
    {
        if (isIdling)
        {
            DoIdle();
        }
        else
        {
            DoWander();
        }

        return NodeStatus.RUNNING;
    }

    // --- Private Methods ---
    private void DoIdle()
    {
        agent.SetDestination(idlePoint.position);
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTimeThreshold)
        {
            isIdling = false;
            idleTimer = 0f;
        }
    }

    private void DoWander()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderTimeThreshold)
        {
            isIdling = true;
            wanderTimer = 0f;
        }
    }
}
