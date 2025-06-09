
using UnityEngine;
using System.Collections.Generic;
using System.Threading;


public class AdvancedBTAgent : MonoBehaviour
{
    public Transform player;
    public Transform idlePoint;
    public Transform[] wanderPoints;

    private BTNode tree;

    private float timeSinceLastPlayerSeen = 0f;
    private float timeCount = 0f;
    private bool reachedIdlePoint = false;
    private bool isChasing = false;
    private bool isWandering = false;

    void Start()
    {
        tree = new SelectorNode(new List<BTNode>
        {
            // 1. CHASE PLAYER
            new SequenceNode(new List<BTNode>
            {
                new IsPlayerVisibleNode(transform, player, 5f),
                new ActionNode(() => {
                    isChasing = true;
                    isWandering = false;
                    Debug.Log("Chasing player");
                    return NodeState.Success;
                }),
                new MoveToTargetNode(transform, player, 5f)
            }),

            // 2. RETURN TO IDLE IF WANDERING TOO LONG
            new SequenceNode(new List<BTNode>
            {
                new HasWanderedTooLongNode(() => timeCount >= 10f),
                new MoveToTargetNode(transform, idlePoint, 1.5f),
                new ActionNode(() => {
                    if (Vector3.Distance(transform.position, idlePoint.position) < 0.1f)
                    {
                        reachedIdlePoint = true;
                        isWandering = false;
                        Debug.Log("Reached idle point!");
                    }
                    return NodeState.Success;
                })
            }),

            // 3. WANDER IF IDLE LONG ENOUGH
            new SequenceNode(new List<BTNode>
            {
                new ActionNode(() => {
                    bool waited = timeSinceLastPlayerSeen >= 3f && reachedIdlePoint;
                    Debug.Log("⏳ HasWaitedLongEnough: " + waited + $" (time={timeSinceLastPlayerSeen:F2}, idle={reachedIdlePoint})");
                    return waited ? NodeState.Success : NodeState.Failure;
                }),
                new ActionNode(() => {
                    isWandering = true;
                    Debug.Log("Wandering...");
                    return NodeState.Success;
                }),
                new WanderNode(transform, wanderPoints, 1.5f)
            }),

            // 4. GO TO IDLE INITIALLY
            new SequenceNode(new List<BTNode>
            {
                new NotAtTargetNode(transform, idlePoint),
                new MoveToTargetNode(transform, idlePoint, 1.5f),
                new ActionNode(() => {
                    if (Vector3.Distance(transform.position, idlePoint.position) < 0.1f)
                    {
                        reachedIdlePoint = true;
                        isWandering = false;
                        Debug.Log("Reached idle point!");
                    }
                    return NodeState.Success;
                })
            }),

            // 5. IDLE NODE (fallback idle behavior)
            new IdleNode(2f)
        });
    }

    void Update()
    {
        if (isChasing)
        {
            timeSinceLastPlayerSeen = 0f;
            timeCount = 0f;
        }
        else
        {
            timeSinceLastPlayerSeen += Time.deltaTime;

            if (reachedIdlePoint && timeSinceLastPlayerSeen >= 3f)
            {
                timeCount += Time.deltaTime;
            }
            else
            {
                timeCount = 0f;
            }
        }

        if (Vector3.Distance(transform.position, idlePoint.position) < 0.1f)
        {
            reachedIdlePoint = true;
        }

        isChasing = false;
        tree.Evaluate();
    }

    private bool IsPlayerVisible()
    {
        return Vector3.Distance(transform.position, player.position) < 5f;
    }
}

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
























//public class AdvancedBTAgent : MonoBehaviour
//{
//    public Transform player;
//    public Transform idlePoint;
//    public Transform[] wanderPoints;

//    private BTNode tree;

//    private float timeSinceLastPlayerSeen = 0f;
//    private float timeCount = 0f;
//    private bool reachedIdlePoint = false;
//    private bool isChasing = false;
//    public bool isWandering = false;

//    void Start()
//    {
//        tree = new SelectorNode(new List<BTNode>
//        {
//            // 1. CHASE PLAYER
//            new SequenceNode(new List<BTNode>
//            {
//                new IsPlayerVisibleNode(transform, player, 5f),
//                new ActionNode(() => {
//                    isChasing = true;
//                    isWandering = false;
//                    Debug.Log("Chasing player");
//                    return NodeState.Success;
//                }),
//                new MoveToTargetNode(transform, player, 5f)
//            }),

//            // 2. RETURN TO IDLE IF WANDERING TOO LONG
//            new SequenceNode(new List<BTNode>
//            {
//                new HasWanderedTooLongNode(() => timeCount >= 10f),
//                new MoveToTargetNode(transform, idlePoint, 1.5f),
//                new ActionNode(() => {
//                    //if (Vector3.Distance(transform.position, idlePoint.position) < 0.1f)
//                    //{
//                    //    reachedIdlePoint = true;
//                    //    isWandering = false;
//                    //    Debug.Log("Reached idle point!");
//                    //}
//                    ReachedIdlePoint();

//                    return NodeState.Success;
//                })
//            }),

//            // 3. WANDER IF IDLE LONG ENOUGH
//            new SequenceNode(new List<BTNode>
//            {
//                new HasWaitedLongEnoughNode(() => {
//                    bool waited = !IsPlayerVisible() && ReachedIdlePoint();
//                    Debug.Log("HasWaitedLongEnough: " + waited + $" (time={timeSinceLastPlayerSeen:F2}, idle={reachedIdlePoint})");
//                    return waited;
//                }),
//                new ActionNode(() => {
//                    Debug.Log("Wandering...");
//                    return NodeState.Success;
//                }),
//                new WanderNode(transform, wanderPoints, 1.5f)
//            }),

//            // 4. GO TO IDLE INITIALLY
//            new SequenceNode(new List<BTNode>
//            {
//                new NotAtTargetNode(transform, idlePoint),
//                new MoveToTargetNode(transform, idlePoint, 1.5f),
//                new ActionNode(() => {
//                    if (ReachedIdlePoint())
//                    //isWandering = false;
//                    {
//                        //reachedIdlePoint = true;                        
//                        //Debug.Log("Reached idle point!");
//                    }
//                    return NodeState.Success;
//                })
//            })
//        });
//    }

//    void Update()
//    {
//        if (isChasing)
//        {
//            timeSinceLastPlayerSeen = 0f;
//            timeCount = 0f;
//        }
//        //else
//        //{
//        //    timeSinceLastPlayerSeen += Time.deltaTime;

//        //    if (reachedIdlePoint && timeSinceLastPlayerSeen >= 3f)
//        //    {
//        //        timeCount += Time.deltaTime;
//        //    }
//        //    else
//        //    {
//        //        timeCount = 0f;
//        //    }
//        //}



//        isChasing = false;
//        tree.Evaluate();
//    }

//    private bool IsPlayerVisible()
//    {
//        return Vector3.Distance(transform.position, player.position) < 5f;
//    }

//    private bool ReachedIdlePoint()
//    {
//        if (Vector3.Distance(transform.position, idlePoint.position) < 0.1f)
//        {
//            reachedIdlePoint = true;
//            isWandering = false;
//            timeSinceLastPlayerSeen += Time.deltaTime;

//            return true;
//        }
//        timeSinceLastPlayerSeen = 0f;
//        return false;
//    }
//}
