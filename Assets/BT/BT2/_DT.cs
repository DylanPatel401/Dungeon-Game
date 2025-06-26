using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A simple Decision Tree implementation for AI behavior.
/// The tree uses a series of if/else checks to decide on an action.
/// </summary>
public class _DT : MonoBehaviour
{
    // --- Public fields for configuration ---
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
        // This is the root of our decision tree. We start here every frame.
        MakeDecision();
    }

    /// <summary>
    /// Evaluates the decision tree from the top down.
    /// </summary>
    private void MakeDecision()
    {
        // --- DECISION 1: Is the enemy in chase range? ---
        if (IsEnemyInChaseRange())
        {
            // --- ACTION: Chase ---
            // If the answer is yes, we take the "Chase" action and stop here.
            DoChase();
        }
        else
        {
            // If the answer is no, we move to the next branch of the tree.
            // --- DECISION 2: Are we currently idling? ---
            if (isIdling)
            {
                // --- ACTION: Idle ---
                DoIdle();
            }
            else
            {
                // --- ACTION: Wander ---
                DoWander();
            }
        }
    }

    /// <summary>
    /// A condition check that returns true if the enemy is close enough to chase.
    /// </summary>
    private bool IsEnemyInChaseRange()
    {
        return Vector3.Distance(transform.position, enemy.position) <= chaseRange;
    }

    // --- ACTION METHODS (The leaves of our tree) ---

    /// <summary>
    /// ACTION: Move towards the enemy.
    /// </summary>
    private void DoChase()
    {
        // Reset timers from other states when we start chasing.
        isIdling = false;
        wanderTimer = 0f;
        idleTimer = 0f;

        agent.SetDestination(enemy.position);
    }

    /// <summary>
    /// ACTION: Move to the idle point and wait.
    /// </summary>
    private void DoIdle()
    {
        agent.SetDestination(idlePoint.position);

        // Check if it's time to stop idling.
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTimeThreshold)
        {
            isIdling = false; // Transition back to wandering
            idleTimer = 0f;
        }
    }

    /// <summary>
    /// ACTION: Patrol between waypoints.
    /// </summary>
    private void DoWander()
    {
        if (waypoints.Length == 0) return;

        // Move to the next waypoint if we've arrived at the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        // Check if it's time to start idling.
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderTimeThreshold)
        {
            isIdling = true; // Transition to idling
            wanderTimer = 0f;
        }
    }
}
