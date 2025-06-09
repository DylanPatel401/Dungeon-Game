using System.Collections.Generic;
public class SelectorNode : BTNode {
    private List<BTNode> children;
    public SelectorNode(List<BTNode> children) { this.children = children; }
    public override NodeState Evaluate() {
        foreach (var child in children) {
            var s = child.Evaluate();
            if (s == NodeState.Success) return NodeState.Success;
            if (s == NodeState.Running) return NodeState.Running;
        }
        return NodeState.Failure;
    }
}
