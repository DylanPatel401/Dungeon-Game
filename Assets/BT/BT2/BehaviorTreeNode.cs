using UnityEngine;
using UnityEngine.AI;

// --- Node Status ---
// Enum to represent the possible states of a node.
public enum NodeStatus
{
    SUCCESS,
    FAILURE,
    RUNNING
}

/// <summary>
/// The base class for all nodes in the Behavior Tree.
/// </summary>
public abstract class Node
{
    // --- Public Properties ---
    public NodeStatus status;

    // --- Public Methods ---
    /// <summary>
    /// Evaluates the node's logic.
    /// This method must be implemented by all derived classes.
    /// </summary>
    /// <returns>The status of the node after evaluation.</returns>
    public abstract NodeStatus Evaluate();
}
