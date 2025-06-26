using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeNode2 : Node
{
    private NavMeshAgent agent;
    private Transform enemy;
    private float fleeDistance = 10f;
    private float safeDistance = 15f;
    private float sampleRadius = 5f;

    public FleeNode2(NavMeshAgent agent, Transform enemy)
    {
        this.agent = agent;
        this.enemy = enemy;
    }

    public override NodeStatus Evaluate()
    {
        float distanceToEnemy = Vector3.Distance(agent.transform.position, enemy.position);

        if (distanceToEnemy >= safeDistance)
        {
            Debug.Log("FleeNode: Reached safe distance. Flee complete.");
            return NodeStatus.SUCCESS;
        }

        Vector3 fleeDirection = (agent.transform.position - enemy.position).normalized;
        Vector3 targetFleePosition = agent.transform.position + fleeDirection * fleeDistance;

        if (NavMesh.SamplePosition(targetFleePosition, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.DrawLine(agent.transform.position, hit.position, Color.cyan);
            Debug.Log($"FleeNode: Fleeing from enemy to {hit.position}");
            return NodeStatus.RUNNING;
        }
        else
        {
            Debug.LogWarning("FleeNode: No valid NavMesh position found.");
            return NodeStatus.FAILURE;
        }
    }
}
