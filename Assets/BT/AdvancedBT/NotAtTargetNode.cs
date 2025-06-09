
using UnityEngine;

public class NotAtTargetNode : BTNode
{
    private Transform agent;
    private Transform target;

    public NotAtTargetNode(Transform agent, Transform target)
    {
        this.agent = agent;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        return Vector3.Distance(agent.position, target.position) > 0.2f
            ? NodeState.Success
            : NodeState.Failure;
    }
}
