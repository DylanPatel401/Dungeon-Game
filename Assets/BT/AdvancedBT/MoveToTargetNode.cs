using UnityEngine;
public class MoveToTargetNode : BTNode {
    Transform agent, target; float speed;
    public MoveToTargetNode(Transform a, Transform t, float s) {
        agent = a; target = t; speed = s;
    }
    public override NodeState Evaluate() {
        agent.position = Vector3.MoveTowards(agent.position, target.position, speed * Time.deltaTime);
        return Vector3.Distance(agent.position, target.position) < 0.1f
            ? NodeState.Success : NodeState.Running;
    }
}
