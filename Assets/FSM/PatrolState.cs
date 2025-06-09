using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private EnemyAI ai;
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int index = 0;
    private float patrolTimer = 0f;
    private const float maxPatrolDuration = 15f; // 10 second limit

    public PatrolState(EnemyAI ai, NavMeshAgent agent, Transform[] waypoints)
    {
        this.ai = ai;
        this.agent = agent;
        this.waypoints = waypoints;
    }

    public void Enter()
    {
        patrolTimer = 0f; // Reset timer when entering patrol
        agent.SetDestination(waypoints[index].position);
    }

    public void Execute()
    {
        // 1. Check for player detection
        //float distToPlayer = Vector3.Distance(ai.transform.position, ai.player.position);
        //if (distToPlayer < ai.chaseRange)
        //{
        //    ai.TransitionToState(ai.chaseState);
        //    return;
        //}

        // 2. Check patrol duration
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= maxPatrolDuration)
        {
            ai.TransitionToState(ai.idleState);
            return;
        }

        // 3. Continue normal patrolling
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            index = (index + 1) % waypoints.Length;
            agent.SetDestination(waypoints[index].position);
        }
    }

    public void Exit()
    {
        // Optional: cleanup or animation
    }
}