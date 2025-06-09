
using System;

public class HasWanderedTooLongNode : BTNode
{
    private Func<bool> condition;

    public HasWanderedTooLongNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        return condition() ? NodeState.Success : NodeState.Failure;
    }
}
