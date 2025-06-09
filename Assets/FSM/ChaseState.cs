using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseState : IState
{
    private EnemyAI ai;
    private NavMeshAgent agent;
    private Transform player;
    private float chaseRange;

    public ChaseState(EnemyAI ai, NavMeshAgent agent, Transform player, float chaseRange)
    {
        this.ai = ai;
        this.agent = agent;
        this.player = player;
        this.chaseRange = chaseRange;
    }

    public void Enter()
    {
        // Optional: trigger chase animation
    }

    public void Execute()
    {
        float dist = Vector3.Distance(ai.transform.position, player.position);

        if (dist > chaseRange * 1.2f)
        {
            ai.TransitionToState(ai.idleState);
            return;
        }

        agent.SetDestination(player.position);
    }

    public void Exit()
    {
        // Optional: end chase animation
    }
}
