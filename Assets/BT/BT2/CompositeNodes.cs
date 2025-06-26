using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// A composite node that executes its children in order until one succeeds.
/// </summary>
public class Selector : Node
{
    // --- Private Fields ---
    private List<Node> children = new List<Node>();

    // --- Constructor ---
    public Selector(List<Node> children)
    {
        this.children = children;
    }

    // --- Public Methods ---
    public override NodeStatus Evaluate()
    {
        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeStatus.SUCCESS:
                    status = NodeStatus.SUCCESS;
                    return status;
                case NodeStatus.RUNNING:
                    status = NodeStatus.RUNNING;
                    return status;
            }
        }

        status = NodeStatus.FAILURE;
        return status;
    }
}

/// <summary>
/// A composite node that executes its children in order until one fails.
/// </summary>
public class Sequence : Node
{
    // --- Private Fields ---
    private List<Node> children = new List<Node>();

    // --- Constructor ---
    public Sequence(List<Node> children)
    {
        this.children = children;
    }

    // --- Public Methods ---
    public override NodeStatus Evaluate()
    {
        bool anyChildRunning = false;
        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeStatus.FAILURE:
                    status = NodeStatus.FAILURE;
                    return status;
                case NodeStatus.RUNNING:
                    anyChildRunning = true;
                    break;
            }
        }

        status = anyChildRunning ? NodeStatus.RUNNING : NodeStatus.SUCCESS;
        return status;
    }
}
