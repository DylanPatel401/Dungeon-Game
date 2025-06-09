using UnityEngine;
public class IsPlayerVisibleNode : BTNode {
    Transform agent, player; float viewDist;
    public IsPlayerVisibleNode(Transform a, Transform p, float d) {
        agent = a; player = p; viewDist = d;
    }
    public override NodeState Evaluate() {
        return Vector3.Distance(agent.position, player.position) < viewDist
            ? NodeState.Success : NodeState.Failure;
    }
}
