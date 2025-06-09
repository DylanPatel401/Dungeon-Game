using System.Collections.Generic;
public class SequenceNode : BTNode {
    private List<BTNode> children;
    public SequenceNode(List<BTNode> children) { this.children = children; }
    public override NodeState Evaluate() {
        bool anyRunning = false;
        foreach (var child in children) {
            var s = child.Evaluate();
            if (s == NodeState.Failure) return NodeState.Failure;
            if (s == NodeState.Running) anyRunning = true;
        }
        return anyRunning ? NodeState.Running : NodeState.Success;
    }
}
