using UnityEngine;
public class WanderNode : BTNode
{
    private Transform agent;
    private Transform[] points;
    private float speed;
    private int currentIndex = 0;

    public WanderNode(Transform agent, Transform[] points, float speed)
    {
        this.agent = agent;
        this.points = points;
        this.speed = speed;
    }

    public override NodeState Evaluate()
    {
        if (points == null || points.Length == 0)
            return NodeState.Failure;

        Transform target = points[currentIndex];
        agent.position = Vector3.MoveTowards(agent.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(agent.position, target.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % points.Length;
            return NodeState.Success; // Let the selector re-evaluate next frame
        }
        return NodeState.Running;
    }
}

