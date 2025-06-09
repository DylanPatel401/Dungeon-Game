
using System;
using UnityEngine;

public class HasWaitedLongEnoughNode : BTNode
{
    private System.Func<bool> condition;

    public HasWaitedLongEnoughNode(System.Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        bool result = condition();
        Debug.Log("HasWaitedLongEnoughNode: " + result);
        return result ? NodeState.Success : NodeState.Failure;
    }
}

