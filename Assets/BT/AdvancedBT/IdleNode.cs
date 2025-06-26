using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : BTNode
{
    private float duration;
    private float startTime;
    private bool isStarted = false;

    public IdleNode(float idleDuration)
    {
        this.duration = idleDuration;
    }

    public override NodeState Evaluate()
    {
        if (!isStarted)
        {
            startTime = Time.time;
            isStarted = true;
            Debug.Log("Entering idle state...");
        }

        if (Time.time - startTime >= duration)
        {
            isStarted = false; // Reset for next time
            Debug.Log("Idle complete.");
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}
