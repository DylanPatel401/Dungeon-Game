
using UnityEngine;
using System.Collections.Generic;

public class BTAgent : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public Transform idlePoint;

    private BTNode tree;

    void Start()
    {
        tree = new SelectorNode(new List<BTNode>
        {
            new SequenceNode(new List<BTNode>
            {
                new IsPlayerVisibleNode(transform, player, 5f),
                new MoveToTargetNode(transform, player, 2f)
            }),
            new MoveToTargetNode(transform, idlePoint, 1f)
        });
    }

    void Update()
    {
        tree.Evaluate();
    }
}
