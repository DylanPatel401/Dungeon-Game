using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A simplified Behavior Tree implementation.
/// The decision-making is handled by a clear priority system in the Update method.
/// </summary>
public class _BT_Simple : MonoBehaviour
{
    // --- Public fields (Same as your FSM) ---
    [Header("AI Configuration")]
    public float chaseRange = 6f;
    public float idleTimeThreshold = 10f;
    public float wanderTimeThreshold = 20f;

    [Header("AI References")]
    public Transform[] waypoints;
    public Transform idlePoint;
    public Transform enemy;

    // --- Private State Variables ---
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float wanderTimer = 0f;
    private float idleTimer = 0f;
    private bool isIdling = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // This 'if/else' block is our Behavior Tree.
        // It checks for the highest priority behavior first (Chasing).

        float enemyDistance = Vector3.Distance(transform.position, enemy.position);

        // 1. HIGHEST PRIORITY: CHASE BEHAVIOR
        if (enemyDistance <= chaseRange)
        {
            DoChase();
        }
        // 2. LOWER PRIORITY: PATROL BEHAVIOR (only runs if we are not chasing)
        else
        {
            DoPatrolCycle();
        }
    }

    /// <summary>
    /// This is our CHASE action.
    /// </summary>
    private void DoChase()
    {
        // When we start chasing, reset the patrol cycle timers.
        // This ensures that when we stop chasing, we start wandering again.
        isIdling = false;
        wanderTimer = 0f;
        idleTimer = 0f;

        agent.SetDestination(enemy.position);
    }

    /// <summary>
    /// This function handles the entire WANDER <-> IDLE cycle.
    /// </summary>
    private void DoPatrolCycle()
    {
        if (isIdling)
        {
            // --- Currently in the "Idle" part of the cycle ---
            agent.SetDestination(idlePoint.position);

            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeThreshold)
            {
                // Idle time is over, switch back to wandering.
                isIdling = false;
                idleTimer = 0f;
            }
        }
        else
        {
            // --- Currently in the "Wander" part of the cycle ---
            Wander(); // This is the original Wander method.

            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderTimeThreshold)
            {
                // Wander time is over, switch to idling.
                isIdling = true;
                wanderTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Moves the agent between waypoints. (Copied from your FSM).
    /// </summary>
    private void Wander()
    {
        if (waypoints.Length == 0) return;

        // If we've reached our destination, get the next one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}