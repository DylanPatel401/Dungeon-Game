using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAtPatrolPointNode : BTNode
{
    private Transform agent;
    private Transform patrolPoint;

    public IsAtPatrolPointNode(Transform agent, Transform patrolPoint)
    {
        this.agent = agent;
        this.patrolPoint = patrolPoint;
    }

    public override NodeState Evaluate()
    {
        return Vector3.Distance(agent.position, patrolPoint.position) < 0.5f
            ? NodeState.Success
            : NodeState.Failure;
    }
}
