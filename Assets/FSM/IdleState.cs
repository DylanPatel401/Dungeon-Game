using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private EnemyAI ai;
    private NavMeshAgent agent;
    private Transform idlePoint;
    private float idleTimer = 0f;
    private const float requiredIdleDuration = 3f; // 3 second wait time

    public IdleState(EnemyAI ai, NavMeshAgent agent, Transform idlePoint)
    {
        this.ai = ai;
        this.agent = agent;
        this.idlePoint = idlePoint;
    }

    public void Enter()
    {
        idleTimer = 0f; // Reset timer when entering idle
        agent.SetDestination(idlePoint.position);
        // Optional: trigger idle animation
    }

    public void Execute()
    {
        //// 1. First check for enemies
        //float distToPlayer = Vector3.Distance(ai.transform.position, ai.player.position);
        //if (distToPlayer < ai.chaseRange)
        //{
        //    ai.TransitionToState(ai.chaseState);
        //    return;
        //}

        // 2. Only count idle time if we've reached the idle point
        if (agent.remainingDistance < 0.1f && !agent.pathPending)
        {
            idleTimer += Time.deltaTime;

            // 3. Transition to patrol after required idle time
            if (idleTimer >= requiredIdleDuration)
            {
                ai.TransitionToState(ai.patrolState);
                return;
            }
        }
    }

    public void Exit()
    {
        // Optional: end idle animation
    }
}